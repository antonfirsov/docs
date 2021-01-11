---
post_title: Migrating RealProxy Usage to DispatchProxy
username: mikerou@microsoft.com
microsoft_alias: mikerou@microsoft.com
categories: .NET Core
desired_publication_date: 1/12/2021
summary: The `RealProxy` type is not available in .NET Core or .NET 5, but other replacement types have been added. This post looks at how DispatchProxy and Castle.Core's DynamicProxy enable AOP paradigms in .NET 5.
---

As I've helped customers port .NET Framework apps to .NET Core 3.1 and .NET 5, one question that has come up several times is the question of what to do about [`System.Runtime.Remoting.Proxies.RealProxy`][real-proxy] usage. Customers using the API are concerned because they know that remoting is [not supported][remoting-unavailable] on .NET Core. Fortunately, the new [`DispatchProxy`][dispatch-proxy] API works as an easy replacement in many cases. In this blog post, I'm going to briefly discuss how to use `DispatchProxy` (or Castle.Core's `DynamicProxy`) in place of `RealProxy` for aspect-oriented programming scenarios and what differences exist between the three proxy APIs.

Despite being in the `System.Runtime.Remoting` namespace, in many cases, most of you aren't actually doing any cross-process remoting with `RealProxy`. Instead, many of you are using it as an easy way to implement an [aspect-oriented programming (AOP)][aop] paradigm as explained in [Aspect-Oriented Programming : Aspect-Oriented Programming with the RealProxy Class][msdn-article]. The idea is that cross-cutting concerns (like logging or caching) can be handled in a proxy class that wraps other types so that those concerns can be handled centrally without having to modify the original classes.

Although `RealProxy` isn't available in .NET Core or .NET 5, a dedicated API was added for exactly this scenario. If you are trying to implement cross-cutting concerns via proxy wrappers (and don't care about cross-process remoting), [`System.Reflection.DispatchProxy`][dispatch-proxy] will probably fit the bill. And if `DispatchProxy` doesn't meet your needs for some reason, the third-party [`Castle.DynamicProxy`][dynamic-proxy] APIs from [Castle.Core][castle-core] offer another .NET Standard-compatible alternative. The sample code in this post is available on GitHub in the [mjrousos/ProxyExploration][github] repository.

## RealProxy example

To start, let's take a look at how `RealProxy` may have been used in a .NET Framework project to add logging.

`RealProxy` is an abstract class. To use it, you need to derive from it and implement the abstract `Invoke` method. The proxy is able to generate objects that appear to be of a given type but, when the generated objects' APIs are used, the proxy's `Invoke` method will be called (instead of using the proxied type's members). `Invoke` should take whatever action is desired (possibly invoking the indicated member on a wrapped instance of the proxied type) and return a `ReturnMessage` with the result of the operation.

Here is a simple example of a `RealProxy`-based proxy for adding [Serilog][serilog] logging around all method calls. The comments in the code explain the role the different methods play in making the proxy work.

```CSharp
// Simple sample RealProxy-based logging proxy
// Note that the proxied type must derive from MarshalByRefObject
public class RealProxyLoggingDecorator<T> : RealProxy where T: MarshalByRefObject
{
    // A field to store an inner 'real' instance of the proxied type.
    // All API calls will go to this wrapped instance (after the proxy has 
    // done its logging).
    private readonly T _target;

    // The Serilog logger to be used for logging.
    private readonly Logger _logger;

    // When creating a new instance of this proxy type, invoke RealProxy's
    // constructor with the type to be wrapped and, optionally, allow
    // the caller to provide a 'target' object to be wrapped.
    private RealProxyLoggingDecorator(T target = null) : base(typeof(T))
    {
        // If no target object is supplied, created a new one.
        if (target == null)
        {
            target = Activator.CreateInstance<T>();
        }

        _target = target;

        // Setup the Serilog logger
        _logger = new LoggerConfiguration()
            .WriteTo.Console().CreateLogger();
        _logger.Information("New logging decorator created for object of type {TypeName}", typeof(T).FullName);
    }

    // This convenience method creates an instance of RealProxyLoggingDecorator
    // and calls RealProxy's GetTransparentProxy method to retrieve the proxy
    // object (which looks like an instance of T but calls our Invoke method
    // whenever an API is used).
    public static T Decorate(T target = null) =>
        new RealProxyLoggingDecorator<T>(target).GetTransparentProxy() as T;

    // The invoke method is the heart of a RealProxy implementation. Here, we
    // define what should happen when a member on the proxy object is used.
    public override IMessage Invoke(IMessage msg)
    {
        // The IMessage argument should be an IMethodCallMessage indicating
        // which method is being invoked. Interestingly, RealProxy even translates 
        // field access into method calls so those will show up here, too.
        if (msg is IMethodCallMessage methodCallMsg)
        {
            try
            {
                // Perform the logging that this proxy is meant to provide
                _logger.Information("Calling method {TypeName}.{MethodName} with arguments {Arguments}",
                    methodCallMsg.MethodBase.DeclaringType.Name, methodCallMsg.MethodName, methodCallMsg.Args);

                // Cache the method's arguments locally so that out and ref args can be updated at invoke time.
                // (methodCallMsg.Args can't be updated directly since they are returned by a property getter
                // and don't refer to a consistent object[])
                var args = methodCallMsg.Args;

                // For this proxy implementation, we still want to call the original API 
                // (after logging has happened), so use reflection to invoke the desired
                // API on our wrapped target object.
                var result = methodCallMsg.MethodBase.Invoke(_target, args);

                // A little more logging.
                _logger.Information("Method {TypeName}.{MethodName} returned {ReturnValue}", 
                    methodCallMsg.MethodBase.DeclaringType.Name, methodCallMsg.MethodName, result);

                // Finally, Invoke should return a ReturnMessage object indicating the result of the operation
                return new ReturnMessage(result, args, args.Length, methodCallMsg.LogicalCallContext, methodCallMsg);
            }
            catch (TargetInvocationException exc)
            {
                // If the MethodBase.Invoke call fails, log a warning and then return
                // a ReturnMessage containing the exception.
                _logger.Warning(exc.InnerException, "Method {TypeName}.{MethodName} threw exception: {Exception}",
                    methodCallMsg.TypeName, methodCallMsg.MethodName, exc.InnerException);

                return new ReturnMessage(exc.InnerException, methodCallMsg);
            }
        }
        else
        {
            throw new ArgumentException("Invalid message; expected IMethodCallMessage", nameof(msg));
        }
    }
}
```

To use the proxy type, a caller just needs to use the `Decorate` helper method to create an instance of the proxy. The returned object will look and act just like the type being proxied except that the proxy's `Invoke` method will be used whenever a member of the object is used.

```CSharp
// Creates an object that acts like an instance of Widget
var widget = RealProxyLoggingDecorator<Widget>.Decorate(new Widget("Widget name", 9.99));
```

### Pros and cons of RealProxy

* **Pros**
  * Has been part of the .NET Framework since 1.0 and Mono since 2.0, so it's available to any .NET Framework app and may be more familiar to .NET developers.
  * Intercepts all member access - even setting or getting field values.
  * Proxies by wrapping the target object, so a proxy can be created around an already existing object.
* **Cons**
  * Can only proxy types derived from `MarshalByRefObject`, so types to be proxied often need to be derived from that type specifically for proxying purposes.
  * Proxies calls to underlying objects by wrapping the target object. Therefore, if the object being wrapped makes subsequent calls to its own APIs as part of executing a method, those member accesses will not be proxied. For example, if `Widget.Buy()` uses `Widget.Price` internally, calling `Buy()` on a proxy object using the sample `RealProxy` implementation above would log the call to `Buy` but not the subsequent call to `Price`.
  * Unavailable in .NET Standard, .NET Core, or .NET 5.

## DispatchProxy as an alternative

[`System.Reflection.DispatchProxy`][dispatch-proxy] was created as a .NET Standard alternative to `RealProxy`. It is a very similar API - it is used by deriving from `DispatchProxy` and implementing an `Invoke` method that is called when a method or property on the proxied type is called. But it also has a few important differences from `RealProxy`. Whereas `RealProxy` only worked with `MarshalByRefObject` types, `DispatchProxy` works based on interfaces. So, target types don't need to derive from `MarshalByRefObject`, but they do need to implement an interface and that interface is what will be exposed by the proxy type created by `DispatchProxy`.

A simple implementation of a logging proxy using `DispatchProxy` might look like this:

```CSharp
// Simple sample DispatchProxy-based logging proxy
public class DispatchProxyLoggingDecorator<T> : DispatchProxy 
    where T: interface // T must be an interface
{
    // The Serilog logger to be used for logging.
    private readonly Logger _logger;

    // Expose the target object as a read-only property so that users can access
    // fields or other implementation-specific details not available through the interface
    public T Target { get; private set; }

    // DispatchProxy's parameterless ctor is called when a 
    // new proxy instance is Created
    public DispatchProxyLoggingDecorator() : base()
    {
        // Setup the Serilog logger
        _logger = new LoggerConfiguration()
            .WriteTo.Console().CreateLogger();
        _logger.Information("New logging decorator created for object of type {TypeName}", typeof(T).FullName);
    }

    // This convenience method creates an instance of DispatchProxyLoggingDecorator,
    // sets the target object, and calls DispatchProxy's Create method to retrieve the 
    // proxy implementation of the target interface (which looks like an instance of T 
    // but calls its Invoke method whenever an API is used).
    public static T Decorate(T target = null)
    {
        // DispatchProxy.Create creates proxy objects
        var proxy = Create<T, DispatchProxyLoggingDecorator<T>>()
            as DispatchProxyLoggingDecorator<T>;

        // If the proxy wraps an underlying object, it must be supplied after creating
        // the proxy.
        proxy.Target = target ?? Activator.CreateInstance<T>();

        return proxy as T;
    }

    // The invoke method is the heart of a DispatchProxy implementation. Here, we
    // define what should happen when a method on the proxied object is used. The
    // signature is a little simpler than RealProxy's since a MethodInfo and args
    // are passed in directly.
    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        try
        {
            // Perform the logging that this proxy is meant to provide
            _logger.Information("Calling method {TypeName}.{MethodName} with arguments {Arguments}", targetMethod.DeclaringType.Name, targetMethod.Name, args);

            // For this proxy implementation, we still want to call the original API 
            // (after logging has happened), so use reflection to invoke the desired
            // API on our wrapped target object.
            var result = targetMethod.Invoke(Target, args);

            // A little more logging.
            _logger.Information("Method {TypeName}.{MethodName} returned {ReturnValue}", targetMethod.DeclaringType.Name, targetMethod.Name, result);

            return result;
        }
        catch (TargetInvocationException exc)
        {
            // If the MethodInvoke.Invoke call fails, log a warning and then rethrow the exception
            _logger.Warning(exc.InnerException, "Method {TypeName}.{MethodName} threw exception: {Exception}", targetMethod.DeclaringType.Name, targetMethod.Name, exc.InnerException);

            throw exc.InnerException;
        }
    }
}
```

Using a proxy object generated by `DispatchProxy.Create` is very similar to using one generated from `RealProxy` except that the type of the object is actually the type of the proxy (not the underlying object), which implements the proxied interface. One of the challenges with this is that fields on the proxied type won't be available (since they won't be present on the interface). To work around this, the proxy can expose the target object or helper methods could be added to the proxy type that allow accessing fields on the target object via reflection, but workarounds like this are a bit messy.

```CSharp
var undecoratedWidget = new Widget("Widgetty", 9.99);

// Note that the proxy is of type IWidget rather than Widget.
// The returned object is actually of type DispatchProxyLoggingDecorator
// (so any helper methods on that type can be used in addition to IWidget APIs)
var widget = DispatchProxyLoggingDecorator<IWidget>.Decorate(undecoratedWidget);
```

### Pros and cons of DispatchProxy

* **Pros**
  * Works with .NET Standard 1.3+ (so it works with both .NET Framework 4.6+ and .NET Core/.NET 5).
  * Does not require proxied types to derive from `MarshalByRefObject`.
  * Proxies by wrapping the target object, so a proxy can be created around an already existing object.
* **Cons**
  * Proxies interfaces, not classes, so proxied types must implement an interface and access to any members not in the interface (like fields) is complicated.
  * Proxies calls to underlying objects by wrapping the target object. Therefore, if the object being wrapped makes subsequent calls to its own APIs as part of executing a method, those member accesses will not be proxied. For example, if `Widget.Buy()` uses `Widget.Price` internally, calling `Buy()` on an object proxied using the sample `RealProxy` implementation earlier would log the call to `Buy` but not the subsequent call to `Price`.

## DynamicProxy as an alternative

In most cases, AOP patterns can be implemented either by `DispatchProxy` or `RealProxy` (if targeting .NET Framework). If those don't meet your needs, though, there are a variety of third party options available. One popular third-party alternative is Castle.Core's [DynamicProxy][dynamic-proxy]. Unlike `DispatchProxy`, `DynamicProxy` [works on .NET Framework 3.5 and 4.0][dynamic-proxy-nuget] (in addition to .NET Standard 1.3), so it can be used on older .NET Framework versions.

DynamicProxy is also unique in that it provides [a couple different proxying models][proxy-models], so you can choose the one that fits your scenario best. It can proxy by wrapping a target type (this is called composition-based proxying) just like `DispatchProxy`. In this model, the proxy type implements an interface and can, optionally, route proxied calls to an internal target object - just like `DispatchProxy`. Alternatively, `DynamicProxy` supports inheritance-based proxying, meaning that the proxy object actually derives from the target type and 'intercepts' API calls by just overriding them. This approach means that subsequent internal calls made by the target object will still be proxied (since the methods are overridden) but it comes with a variety of downsides, as well. First, inheritance-based proxying means that only virtual methods can be proxied and, secondly, because the proxy and the target object are one in the same, it's not possible to create a proxy wrapper around an object that already exists. For a more in-depth look at `DynamicProxy`, check out [this tutorial][dynamic-proxy-tutorial].

When using `DynamicProxy`, you create `IInterceptor` types that are responsible for handling calls to the proxy object. A simple logging interceptor might look like this:

```CSharp
// Simple sample logging interceptor for DynamicProxy
class DynamicProxyLoggingInterceptor : IInterceptor
{
    // The Serilog logger to be used for logging.
    private readonly Logger _logger;

    // Constructor that initializes the Logger the interceptor will use
    public DynamicProxyLoggingInterceptor(string typeName = null) 
    {
        // Setup the Serilog logger
        _logger = new LoggerConfiguration()
            .WriteTo.Console().CreateLogger();
        _logger.Information($"New logging decorator created{(string.IsNullOrWhiteSpace(typeName) ? string.Empty : " for object of type {TypeName}")}", typeName);
    }

    // The Intercept method is where the interceptor decides how to handle calls to the proxy object
    public void Intercept(IInvocation invocation)
    {
        try
        {
            // Perform the logging that this proxy is meant to provide
            _logger.Information("Calling method {TypeName}.{MethodName} with arguments {Arguments}", invocation.Method.DeclaringType.Name, invocation.Method.Name, invocation.Arguments);

            // Invocation.Proceeds goes on to the next interceptor or, if there are no more interceptors, invokes the method.
            // The details of how the method are invoked will depend on the proxying model used. The interceptor does not need
            // to know those details.
            invocation.Proceed();

            // A little more logging.
            _logger.Information("Finished calling method {TypeName}.{MethodName}", invocation.Method.DeclaringType.Name, invocation.Method.Name);
        }
        catch (TargetInvocationException exc)
        {
            // If the subsequent invocation fails, log a warning and then rethrow the exception
            _logger.Warning(exc.InnerException, "Method {TypeName}.{MethodName} threw exception: {Exception}", invocation.Method.DeclaringType.Name, invocation.Method.Name, exc.InnerException);

            throw exc.InnerException;
        }
    }
}
```

The proxy object itself is created by calling methods on the `ProxyGenerator` type. [Different APIs][dynamic-proxy-apis] are used depending on the proxying model being used. Helper methods for creating an inheritance-based proxy and composition-based proxy might look like this:

```CSharp
public class DynamicProxyLoggingDecorator
{
    // ProxyGenerator is used to create DynamicProxy proxy objects
    private static readonly ProxyGenerator _generator = new ProxyGenerator();

    // CreateClassWithProxy uses inheritance-based proxying to create a new object that actually
    // derives from the provided class and applies interceptors to all APIs before
    // invoking the base class's implementation. In this model, the proxy
    // object and target object are the same.
    public static T DecorateViaInheritance<T>() where T: class =>
        _generator.CreateClassProxy<T>(new DynamicProxyLoggingInterceptor(typeof(T).Name));

    // CreateInterfaceProxyWithTarget uses composition-based proxying to wrap a target object with
    // a proxy object implementing the desired interface. Calls are passed to the target object
    // after running interceptors. This model is similar to DispatchProxy.
    public static T DecorateViaComposition<T>(T target = null) where T: class
    {
        var proxy = target != null ?
            _generator.CreateInterfaceProxyWithTarget(target, new DynamicProxyLoggingInterceptor(typeof(T).Name)) :

            // There is also a CreateInterfaceProxyWithoutTarget method but that API assumes there is no wrapped object at all,
            // and only the interceptors are called. That model can be useful but doesn't match the logging scenario used in this sample.
            _generator.CreateInterfaceProxyWithTarget<T>(Activator.CreateInstance(typeof(T)) as T, new DynamicProxyLoggingInterceptor(typeof(T).Name));

        return proxy;
    }
}
```

### Pros and cons of DynamicProxy

* **Pros**
  * Works on both .NET Core/.NET 5 and .NET Framework 3.5+
  * Support for multiple proxying patterns adds flexibility
    * The inheritance-based proxy option enables methods called from within a proxy object to also be proxied (though this has its own drawbacks)
  * Does not require `MarshalByRefObjects`; can work with interfaces or classes with virtual members
  * The `IInterceptor` model makes it easy to combine multiple proxy behaviors
* **Cons**
  * Proxies are based on interfaces or virtual members on classes, so proxied types must implement interesting APIs according to one of those patterns
  * Unlike `RealProxy`, cannot proxy field access, though inheritance-based proxying at least allows access to fields more easily than `DispatchProxy` (interceptors are not invoked)
  * More complicated than other options (multiple proxy creation methods, `IInterceptor`, etc.)

## Summary and Wrap-up

The different proxy APIs discussed above all have their own pros and cons. None will be right for all scenarios, but hopefully this post helps explain how AOP patterns can still be used in .NET 5 without needing `System.Runtime.Remoting.Proxies.RealProxy`. In most cases, `DispatchProxy` can be dropped in as a straight-forward replacement. And if an inheritance-based proxying pattern would be more useful, or you need the flexibility of the `IInterceptor` model or the ability to proxy virtual methods not from an interface, Castle.Core's `DynamicProxy` is a great option.

As mentioned earlier, the code snippets from this blog post are available (in the context of a sample project exercising the different proxy APIs) on [GitHub].

[real-proxy]: https://docs.microsoft.com/dotnet/api/system.runtime.remoting.proxies.realproxy
[remoting-unavailable]: https://docs.microsoft.com/dotnet/core/porting/net-framework-tech-unavailable#remoting
[aop]: https://wikipedia.org/wiki/Aspect-oriented_programming
[decorator-pattern]: https://wikipedia.org/wiki/Decorator_pattern
[msdn-article]: https://docs.microsoft.com/archive/msdn-magazine/2014/february/aspect-oriented-programming-aspect-oriented-programming-with-the-realproxy-class
[dispatch-proxy]: https://www.nuget.org/packages/System.Reflection.DispatchProxy/4.5.1
[dynamic-proxy]: http://www.castleproject.org/projects/dynamicproxy/
[serilog]: https://serilog.net/
[github]: https://github.com/mjrousos/ProxyExploration
[dynamic-proxy-nuget]: https://www.nuget.org/packages/Castle.Core/
[proxy-models]: https://github.com/castleproject/Core/blob/master/docs/dynamicproxy-kinds-of-proxy-objects.md#composition-based
[dynamic-proxy-tutorial]: http://kozmic.net/dynamic-proxy-tutorial/
[dynamic-proxy-apis]: https://github.com/castleproject/Core/blob/master/docs/dynamicproxy-kinds-of-proxy-objects.md
[castle-core]: https://www.nuget.org/packages/Castle.Core
