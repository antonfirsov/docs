---
post_title: What's new for WPF in .NET 7
author1: pchaurasia
author2: fizaazmi
author3: singhashish
post_slug: wpf-on-dotnet-7
username: pchaurasia
microsoft_alias: pchaurasia
featured_image: wpf.JPG
categories: .NET,  WPF, XAML
tags: .net 7, wpf
summary: Find out what's new in WPF on .NET 7 including accessibility improvements, a focus on performance, and much more.
desired_publication_date: 2022-11-09
post_date: 2022-11-09 08:59:10
---

WPF community is filled with so many passionate individuals with truly amazing experiences and this post aims to highlight what has been done in the dotnet/wpf repo in past few months and thanking the amazing people behind all this. We are really grateful for the contributors who have consistently worked towards improving WPF. Here is a quick recap of what was accomplished in the past few months in the dotnet/wpf repository.

### **Performance**

WPF in .NET 7 ships with number of improvements in the areas not just limited to unnecessary boxing/unboxing, use of `Span` for string manipulation, better allocation/deallocation of objects, memory improvements, font rendering etc. but also code cleanup and making way for future readiness.

#### Boxing/Unboxing

 Boxing and Unboxing are computationally expensive processes. When a value type is boxed, a new object must be allocated and constructed. To a lesser degree, the cast required for unboxing is also expensive computationally. 

- [Avoid boxing when setting DependencyObject properties](https://github.com/dotnet/wpf/pull/4220) _(Thanks [Ilya](https://github.com/i-kostikov))_
- [Stop boxing WeakReferenceListEnumerator in PresentationSource use](https://github.com/dotnet/wpf/pull/6502)
- [Reuse bool box objects in UncommonField<bool>.SetValue](https://github.com/dotnet/wpf/pull/6528)
- [Stop boxing in Visual.SetDpiScaleVisualFlags](https://github.com/dotnet/wpf/pull/6309)
- [Avoid enumerator boxing in XamlSchemaContext.UpdateNamespaceByUriList](https://github.com/dotnet/wpf/pull/6282)
- [Avoid boxing list/array enumerator in CreateTextLSRuns](https://github.com/dotnet/wpf/pull/6281)
- [Avoid boxing list enumerator in XamlObjectWriter.Logic_ConvertPositionalParamsToArgs](https://github.com/dotnet/wpf/pull/6279)


#### Allocations

The more objects allocated on the heap, more is GC overhead in reclaiming those object post their lifetimes. Reducing such allocations in memory lowers GC overhead.

- [Avoid Hashtable-related allocations in AccessorTable](https://github.com/dotnet/wpf/pull/6500)
- [Avoid Hashtable-related allocations in DataBindEngine](https://github.com/dotnet/wpf/pull/6501)
- [Remove closure/delegate allocation in ItemContainerGenerator](https://github.com/dotnet/wpf/pull/6396)
- [Avoid delegate allocation to call ListCollectionView.PrepareComparer](https://github.com/dotnet/wpf/pull/6511)
- [Avoid unnecessary byte[] allocation in Baml2006Reader.Process_Header](https://github.com/dotnet/wpf/pull/6276)
- [Avoid allocating `Stack<BranchNode>` just to peek at it](https://github.com/dotnet/wpf/pull/6518)
- [Stop allocating unnecessary StringBuilders in ParsePropertyComments](https://github.com/dotnet/wpf/pull/6508)
- [Avoid unnecessary StringBuilder reallocation in LookupAndSetLocalizabilityAttribute](https://github.com/dotnet/wpf/pull/6509)
- [Avoid exceptional string allocation in StaticExtension.ProvideValue](https://github.com/dotnet/wpf/pull/6269)
- [Remove unnecessary string and string[] allocations from MS.Internal.ContentType](https://github.com/dotnet/wpf/pull/6268)
- [Remove some unnecessary StringBuilders](https://github.com/dotnet/wpf/pull/6275)
- [Remove substring allocation from Baml2006Reader.Logic_GetFullXmlns](https://github.com/dotnet/wpf/pull/6271)
- [Don't allocate fallback name in XamlNamespace.GetXamlType unless it's needed](https://github.com/dotnet/wpf/pull/6270)
- [Avoid unnecessary enumerator allocations in XamlDirective.GetHashCode](https://github.com/dotnet/wpf/pull/6265)


#### Miscellaneous

- [Avoid excessive calls to the PropertyValues index getter.](https://github.com/dotnet/wpf/pull/6293) _(Thanks [paulozemek](https://github.com/paulozemek))_
- [Harden events against race conditions](https://github.com/dotnet/wpf/pull/5722) _(Thanks [Bruno Martinez](https://github.com/brunom) )_
- [Eliminate memory copy when reading font data](https://github.com/dotnet/wpf/pull/6254) _(Thanks [Bradley Grainger](https://github.com/bgrainger))_
- [Small performance improvement of PathParser](https://github.com/dotnet/wpf/pull/4208) _(Thanks [ThomasGoulet73](https://github.com/ThomasGoulet73))_
- [Use span slice instead of substring in AbbreviatedGeometryParser.ReadNumber](https://github.com/dotnet/wpf/pull/6272)
- [Avoid unnecessary duplication of fields in NullableBooleanBoxes](https://github.com/dotnet/wpf/pull/6529)
- [Change most non-generic sorts to be generic](https://github.com/dotnet/wpf/pull/6285)
- [Some improvements to FrugalList](https://github.com/dotnet/wpf/pull/6280)
- [Avoid unnecessary `Collection<T>` wrapper in CombineSources](https://github.com/dotnet/wpf/pull/6517)
- [Avoid unnecessary `List<>` wrapper in GetTextRunSpans](https://github.com/dotnet/wpf/pull/6516)

_Special thanks to [Stephen Toub](https://github.com/stephentoub) for contributing many other performance fixes._ 

### **Accessibility**

With a commitment to ensure WPF controls are accessible, below are the product improvements that made its way to WPF.

- [WPF DataGrid/GridView column width can be changed using the keyboard shortcut ALT+left or right arrow key](https://github.com/dotnet/wpf/pull/6812) - Datagrid column width can be adjusted using keyboard shortcut Alt + Left arrow / Right arrow.
- [Sort Datagrid Column by Keyboard F3](https://github.com/dotnet/wpf/pull/6873) - Datagrid columms can now be sorted (if sorting is enabled for a column) using keyboard shortcut F3.
- [Narrator announcement for Checkable Menuitems](https://github.com/dotnet/wpf/pull/6706) - Onscreen narrators can correctly announce the presence of checkable menuitems.

### **Bug fixes**

While WPF remains fully supported and serviced on .NET Framework, most fixes and all new features will go only into .NET Core, where we have the opportunity to make bigger changes. Our community helped address some long-standing bugs in this release. 

- [FocusVisualStyle can't be overwritten globally](https://github.com/dotnet/wpf/issues/1164) _(Thanks [Bastian Schmidt](https://github.com/batzen))_
- [CommandParameter invalidates CanExecute by miloush](https://github.com/dotnet/wpf/pull/4217) _(Thanks [Jan Kučera](https://github.com/miloush))_
- Tooltip issues
   - [.NET 6 Tooltip behavior change from .NET 5 (bug?)](https://github.com/dotnet/wpf/issues/5703)
   - [Comboboxitem tooltip bug](https://github.com/dotnet/wpf/issues/5716)
- [ContextMenu stops working if its owner is removed from the visual tree](https://github.com/dotnet/wpf/issues/5835)
- [Fixes rounding error while glyphrun serialization](https://github.com/dotnet/wpf/issues/6295) 

The above list is **NOT** exhaustive and many more bug fixes went thanks to our community contributors for their efforts in fixing them.


### **Infrastructure upgrades**

One of the major areas for improvement voiced by community is the rate at which community contributions were accepted. With the below infrastructure upgrades, we intend to address the rate at which we accept community contributions.



| Title   | Description |
| ------------- |:-------------|
| Test repository migration      | WPF codebase has more than 30K integration tests. These tests ensure sanity of the build with various OS and .NET framework combination matrix. As part of open sourcing the test infrastructure, we aim at moving all the tests from internal to Github. This would also enable community to add their own tests to the test repo.<br/> <br/> We have [open-sourced](https://github.com/dotnet/wpf-test) most of the basic regression tests that enables the community contributors to run those tests locally and debug them, in case there's an issue with any of the PR submissions .    |
| Running basic tests on each incoming PR      | Basic regression tests, _(aka Daily Regression Tests or DRT(s))_ that validates the basic behavior of controls, need to be run on each submitted PR to ensure that the changes do not cause any regressions. The end goal of this exercise is to make sure that we have a framework in place that allows running tests on incoming PRs. This would reduce the turn-around times on PRs, thereby increasing the velocity at which new changes / fixes can be merged into the repository.<br /><br />We are now running these tests as a part of build on every incoming PR. In upcoming months, we plan to enhance these pipelines in terms of the reporting the status if test execution, to avoid manual lookup in logs for failures.     |

### Ongoing activities

- Tests repository migration still has a larger subset of tests that are yet to be ported. 
- Enabling running the bigger suite of test cases on community submitted PRs which would improve the turnaround time for feedback on PRs.
- Clearing the backlog of PRs and issues.


## Community
The community run-project started with the intent to enable developers in making big difference in shaping WPF going forward. To all the WPF developers, your work is invaluable. We'd like to thank the below contributors for their efforts in fixing long standing issues and contributing performance and functional improvements.

----
- [Andrii Kurdiumov](https://github.com/kant2002)
    - [Fix ZeroForNow parameter](https://github.com/dotnet/wpf/pull/6657) 
    - [Remove sorcery for getting consistent names in the traces](https://github.com/dotnet/wpf/pull/6656) 
    - [Explicit delegate types](https://github.com/dotnet/wpf/pull/5954) 
    - [Update MSBuild tasks](https://github.com/dotnet/wpf/pull/6034) 
    - [Improve Linux build](https://github.com/dotnet/wpf/pull/5964) 
    - [Remove flacky and redundant codegen](https://github.com/dotnet/wpf/pull/4909) 		
    
---
- [Austin Wise](https://github.com/AustinWise)
    - [Fix setting DPI awareness](https://github.com/dotnet/wpf/pull/6245)

---
- [Bastian Schmidt](https://github.com/batzen)
    - [Fixing TextBoxView memory leak for 2 seconds after unloading host control](https://github.com/dotnet/wpf/pull/1161) 
    - [Use regular resource lookup for FocusVisualStyle](https://github.com/dotnet/wpf/pull/1165) 
---

- [Bradley Grainger](https://github.com/bgrainger)
    - [Eliminate memory copy when reading font data](https://github.com/dotnet/wpf/pull/6254) 
    
---

- [Bruno Martinez](https://github.com/brunom) 
    - [Harden events against race conditions](https://github.com/dotnet/wpf/pull/5722) 

---
- [Ilya](https://github.com/i-kostikov)
    - [Avoid boxing when setting DependencyObject properties](https://github.com/dotnet/wpf/pull/4220)

---
- [Jan Kučera](https://github.com/miloush)

    - [Check script of combining marks during font fallback](https://github.com/dotnet/wpf/pull/6857) 
    - [CommandParameter invalidates CanExecute](https://github.com/dotnet/wpf/pull/4217) 
    - [Ignore NotImplementedException from ITaskbarList](https://github.com/dotnet/wpf/pull/6547) 

---

- [lindexi](https://github.com/lindexi)
    - [Fix the stream do not be closed in ImageSourceTypeConverter](https://github.com/dotnet/wpf/pull/7091) 
    - [Fix GetStreamCore in ContentFilePart](https://github.com/dotnet/wpf/pull/5066) 
    - [Fix create BitmapDecoder with async file stream.](https://github.com/dotnet/wpf/pull/4966) 
    - [Using the `Clone` method to fast clone the array in StylusPoint](https://github.com/dotnet/wpf/pull/5218) 
    - [Using ArrayPool in RenderData](https://github.com/dotnet/wpf/pull/5392) 
    - [Using `Array.Copy` to make array copy faster in StylusPointCollection](https://github.com/dotnet/wpf/pull/5217) 
    - [Use pattern matching in TaskExtensions](https://github.com/dotnet/wpf/pull/4424) 

---

- [paulozemek](https://github.com/paulozemek)
    - [Avoid excessive calls to the PropertyValues index getter.](https://github.com/dotnet/wpf/pull/6293)
---

- [Pomain](https://github.com/pomianowski)
    - [Remove 'Zero Width No-Brake Space' character from multiple files](https://github.com/dotnet/wpf/pull/6570) 
---

- [ThomasGoulet73](https://github.com/ThomasGoulet73)
    - [Small performance improvement of PathParser](https://github.com/dotnet/wpf/pull/4208) 
    - [Use params and char overload](https://github.com/dotnet/wpf/pull/4231) 
    - [Inline VerifyAccess](https://github.com/dotnet/wpf/pull/4021) 
    - [Fix Invalid_IInputElement resource](https://github.com/dotnet/wpf/pull/6691) 
    - [Disable Indeterminate animation when hiding ProgressBar](https://github.com/dotnet/wpf/pull/6266) 
    - [Replace IsAssignableFrom with is in converters](https://github.com/dotnet/wpf/pull/5933) 
    - [Use generic Marshal.StructureToPtr](https://github.com/dotnet/wpf/pull/6044) 
    - [Migrate DPI awareness initialization to managed](https://github.com/dotnet/wpf/pull/5765) 
    - [Use generic Marshal.PtrToStructure](https://github.com/dotnet/wpf/pull/4917) 

---
## Summary

We'd encourage you to try out WPF on .NET 7 and let us know how these improvements have helped.  We are always looking for feedback on how to improve the product and look forward to your contributions. We would like to thank everyone that is committed to making WPF better as a product. Our goal is to continue improving WPF, while growing our community so that we can bring you the best developer experience possible. Your help and input is very much required. Whether that is through triaging issues, updating documentation, participating in discussions or writing code, we appreciate all of your help!
 
