
# .NET Framework 4.6.2 Preview
We are pleased to announce the preview release for the .NET Framework version 4.6.2. The preview release can be downloaded now at one of the following links:

- [.NET Framework 4.6.2 Preview - Standalone](http://go.microsoft.com/fwlink/?LinkID=708750)
- [.NET Framework 4.6.2 Preview - Web Bootstrapper](http://go.microsoft.com/fwlink/?LinkID=708735)
- [.NET Framework 4.6.2 Preview - Developer Pack](http://go.microsoft.com/fwlink/?LinkID=708774)

# Providing Feedback

We hope to hear your feedback as you try the preview release which can be directed to:

- [Bugs - VS Feedback](https://connect.microsoft.com/VisualStudio/Feedback)
- [Suggestions - User Voice](https://visualstudio.uservoice.com/forums/121579-visual-studio-2015)

## ClickOnce

### Transport Layer Security (TLS) 1.1 and 1.2 Support
ClickOnce has been updated to support TLS 1.1 and 1.2. ClickOnce will automatically detect which TLS protocol is required at runtime. There are no extra steps that are needed within the ClickOnce application to enable this. 

ClickOnce continues to support TLS 1.0 for the foreseeable future, even though it is [no longer considered acceptable for PCI Compliance](http://blog.pcisecuritystandards.org/migrating-from-ssl-and-early-tls).

A release to support TLS 1.1 and 1.2 for .NET Framework versions 4.5.2, 4.6 and 4.6.1 as well as Windows 7 and above is planned for  April 2016. 

## Converting Your Desktop App to UWP (Project Centennial)

Previously known as “Project Centennial”, Windows now offers capabilities to bring existing Windows desktop apps (including WPF/Windows Forms) to the Universal Windows Platform (UWP). The goal of this technology is to act as a bridge by enabling developers to gradually migrate their existing code base to UWP, bringing their app to all Windows 10 devices. 

Converted desktop apps will gain an app identity, similar to the app identity of UWP apps, resulting in UWP APIs becoming accessible to enable features such as Live Tiles and notifications. The app will continue to behave as before, running as a full trust app. Once converted, an app container process can be added to the existing full trust process to add an adaptive user interface. When all functionality is moved to the app container process, the full trust process can be removed and the now UWP app can be made available to all Windows 10 devices.

## Cryptography

### X509 Certificates Now Support FIPS 186-3 DSA

The .NET Framework 4.6.2 adds support for DSA (Digital Signature Algorithm) X509 certificates whose keys exceed the FIPS 186-2 limit of 1024-bit.

In addition to supporting the larger key sizes of FIPS 186-3, the .NET Framework 4.6.2 allows computing signatures with the SHA-2 family of hash algorithms (SHA256, SHA384, and SHA512). The FIPS 186-3 support is provided by the new [DSACng class](https://msdn.microsoft.com/en-us/library/system.security.cryptography.dsacng).

Keeping in line with recent changes to RSA (.NET Framework 4.6) and ECDsa (.NET Framework 4.6.1), the DSA abstract base class has additional methods to allow callers to make use of this functionality without casting.

<script src="https://gist.github.com/staceyhaffner/e214f78ff29adf165fb4.js"></script>

### Increased Clarity for Inputs to ECDiffieHellman Key Derivation Routines

.NET Framework version 3.5 added support for [Ellptic Curve Diffie-Hellman](https://msdn.microsoft.com/en-us/library/system.security.cryptography.ecdiffiehellman.aspx) Key Agreement that included three different KDF (Key Derivation Function) routines. The inputs to the routines, and the routine itself, were configured via properties on the ECDiffieHellmanCng object; but since not every routine read every input property there was ample room for confusion.

The ECDiffieHellman base class has been updated to more clearly represent these KDF routines and their inputs: 

<script src="https://gist.github.com/staceyhaffner/71710b339cca406629ad.js"></script>

### Support for Persisted-Key Symmetric Encryption

The Windows Cryptography Library (CNG) has support for storing persisted symmetric keys on software and hardware devices and the .NET Framework 4.6.2 has made it possible for users to make use of this feature. Since key names and key providers is implementation-specific, using this feature requires calling the constructor of the concrete implementation type instead of the more common factory approach (e.g. [Aes.Create()](https://msdn.microsoft.com/en-us/library/bb337875.aspx)).

Persisted-key symmetric encryption support exists for the AES ([AesCng](https://msdn.microsoft.com/en-us/library/system.security.cryptography.aescng.aspx)) and 3DES ([TripleDESCng](https://msdn.microsoft.com/en-us/library/system.security.cryptography.tripledescng.aspx)) algorithms.

<script src="https://gist.github.com/staceyhaffner/96ed50b66ea27cc14ac2.js"></script>

### SignedXml Support for SHA-2 Hashing

The .NET Framework 4.6.2 has added support to SignedXml which permits [RSA-SHA256](https://msdn.microsoft.com/en-us/library/system.security.cryptography.xml.signedxml.xmldsigrsasha256url.aspx), [RSA-SHA384](https://msdn.microsoft.com/en-us/library/system.security.cryptography.xml.signedxml.xmldsigrsasha384url.aspx), and [RSA-SHA512](https://msdn.microsoft.com/en-us/library/system.security.cryptography.xml.signedxml.xmldsigrsasha512url.aspx) PKCS#1 signature methods, and [SHA256](https://msdn.microsoft.com/en-us/library/system.security.cryptography.xml.signedxml.xmldsigsha256url.aspx), [SHA384](https://msdn.microsoft.com/en-us/library/system.security.cryptography.xml.signedxml.xmldsigsha384url.aspx), and [SHA512](https://msdn.microsoft.com/en-us/library/system.security.cryptography.xml.signedxml.xmldsigsha512url.aspx) reference digest algorithms.

The URI constants are all exposed on SignedXml:

<script src="https://gist.github.com/staceyhaffner/3137806c60dd3682ca59.js"></script>

Any programs which have registered a custom SignatureDescription handler into CryptoConfig to add support for these algorithms will continue to function as they did in the past, but since there are now platform defaults the CryptoConfig registration should no longer be necessary.

<script src="https://gist.github.com/staceyhaffner/8bd7376597d54f0c95be.js"></script>

Windows Presentation Foundation
-----------

### Soft Keyboard Support
Soft Keyboard support enables automatic invocation and dismissal of the touch keyboard in WPF applications without disabling WPF stylus/touch support on Windows 10. Prior to 4.6.2, WPF applications do not implicitly support the invocation or dismissal of the touch keyboard without disabling WPF stylus/touch support.  This is due to a change in the way the touch keyboard tracks focus in applications starting in Windows 8.

###  Per-Monitor DPI Support
WPF applications are [system-DPI aware](https://msdn.microsoft.com/en-us/library/windows/desktop/dn280512%28v=vs.85%29.aspx), which means that applications are scaled by Windows depending on the DPI of the monitor on which the application is being rendered. This can result in loss of sharpness, blurry text etc. Prior to 4.6.2, [additional native code](https://msdn.microsoft.com/en-us/library/windows/desktop/ee308410%28v=vs.85%29.aspx) was required to enable per-monitor DPI awareness in WPF applications.

Given the recent proliferation of high-DPI and hybrid-DPI environments in the ecosystem, we have now enabled per-monitor DPI awareness in WPF applications. See the [samples and developer guide](https://github.com/rohit21agrawal/WPF-Samples/tree/master/PerMonitorDPI) for more information about how to enable you WPF application to become per-monitor DPI aware. 