
Download the latest releases now:

- [.NET Framework 4.6.2](http://link)
- [.NET Core](http://link)
- [ASP.NET Core](http://link)

Providing Feedback
--------------------
We hope to hear your feedback as you try the new releases. Feedback can be directed to:

| Technology     | Bugs     |    Suggestions |
| ---------------|-------------------|----------------|
| .NET Framework 4.6.2 | [VS Feedback](https://connect.microsoft.com/VisualStudio/Feedback) | [User Voice](https://visualstudio.uservoice.com/forums/121579-visual-studio-2015) |
| .NET Core      | [GitHub](https://github.com/Microsoft/dotnet) | [GitHub](https://github.com/Microsoft/dotnet) |

# .NET Framework 4.6.2 

## ClickOnce

### Transport Layer Security (TLS) 1.1 and 1.2 Support
ClickOnce has been updated to support TLS 1.1 and 1.2. ClickOnce will automatically detect which TLS protocol is required at runtime. There are no extra steps that are needed within the ClickOnce application to enable this. 

ClickOnce continues to support TLS 1.0 for the foreseeable future, even though it is [no longer considered acceptable for PCI Compliance](http://blog.pcisecuritystandards.org/migrating-from-ssl-and-early-tls).

Support for .NET Framework versions 4.5.2, 4.6 and 4.6.1 as well as Windows 7 and above will be made available in April 2016. 

## Converting Your Desktop App to UWP (Project Centennial)

Previously known as “Project Centennial”, Windows now offers capabilities to bring existing Windows desktop apps (including WPF/Windows Forms) to the Universal Windows Platform (UWP). The goal of this technology is to act as a bridge by enabling developers to gradually migrate their existing code base to UWP, bringing their app to all Windows 10 devices. 

Converted desktop apps will gain an app identity, similar to the app identity of UWP apps, resulting in UWP APIs becoming accessible to enable features such as Live Tiles and notifications. The app will continue to behave as before, running as a full trust app. Once converted, an app container process can be added to the existing full trust process to add an adaptive user interface. When all functionality is moved to the app container process, the full trust process can be removed and the now UWP app can be made available to all Windows 10 devices.

## Cryptography


### X509 Certificates Now Support FIPS 186-3 DSA

The .NET Framework 4.6.2 adds support for DSA (Digital Signature Algorithm) X509 certificates whose keys exceed the FIPS 186-2 limit of 1024-bit.

In addition to supporting the larger key sizes of FIPS 186-3, the .NET Framework 4.6.2 allows computing signatures with the SHA-2 family of hash algorithms (SHA256, SHA384, and SHA512). The FIPS 186-3 support is provided by the new DSACng class.

Keeping in line with recent changes to RSA (.NET Framework 4.6) and ECDsa (.NET Framework 4.6.1), the DSA abstract base class has additional methods to allow callers to make use of this functionality without casting.

```csharp
public static byte[] SignDataDsaSha384(byte[] data, X509Certificate2 cert)
{
    using (DSA dsa = cert.GetDSAPrivateKey())
    {
        return dsa.SignData(data, HashAlgorithmName.SHA384);
    }
}

public static void VerifyDataDsaSha384(byte[] data, byte[] signature, X509Certificate2 cert)
{
    using (DSA dsa = cert.GetDSAPublicKey())
    {
        return dsa.VerifyData(data, signature, HashAlgorithmName.SHA384);
    }
}
```

### Increased Clarity for Inputs to ECDiffieHellman Key Derivation Routines

.NET Framework version 3.5 added support for Ellptic Curve Diffie-Hellman Key Agreement that included three different KDF (Key Derivation Function) routines. The inputs to the routines, and the routine itself, were configured via properties on the ECDiffieHellmanCng object; but since not every routine read every input property there was ample room for confusion.

The ECDiffieHellman base class has been updated to more clearly represent these KDF routines and their inputs: 

```csharp
/// <summary>
/// Derive key material using the formula HASH(secretPrepend || x || secretAppend) where x is the computed
/// result of the EC Diffie-Hellman algorithm.
/// </summary>
public virtual byte[] DeriveKeyFromHash(ECDiffieHellmanPublicKey otherPartyPublicKey, HashAlgorithmName hashAlgorithm, byte[] secretPrepend, byte[] secretAppend)

/// <summary>
/// Derive key material using the formula HMAC(hmacKey, secretPrepend || x || secretAppend) where x is the computed
/// result of the EC Diffie-Hellman algorithm.
/// </summary>
public virtual byte[] DeriveKeyFromHmac(ECDiffieHellmanPublicKey otherPartyPublicKey, HashAlgorithmName hashAlgorithm, byte[] hmacKey, byte[] secretPrepend, byte[] secretAppend)

/// <summary>
/// Derive key material using the TLS pseudo-random function (PRF) derivation algorithm.
/// </summary>
public virtual byte[] DeriveKeyTls(ECDiffieHellmanPublicKey otherPartyPublicKey, byte[] prfLabel, byte[] prfSeed)
```

### Support for Persisted-Key Symmetric Encryption

The Windows Cryptography Library (CNG) added support for storing persisted symmetric keys and using hardware-stored symmetric keys; and the .NET Framework 4.6.2 has made it possible for users to make use of this feature.  Since the notion of key names and key providers is implementation-specific, using this feature requires utilizing the constructor of the concrete implementation types instead of the preferred factory approach (e.g. Aes.Create()).

Persisted-key symmetric encryption support exists for the AES (AesCng) and 3DES (TripleDESCng) algorithms.

```csharp
public static byte[] EncryptDataWithPersistedKey(byte[] data, byte[] iv)
{
    using (Aes aes = new AesCng("AesDemoKey", CngProvider.MicrosoftSoftwareKeyStorageProvider))
    {
        aes.IV = iv;
       
        // Using the zero-argument overload is required to make use of the persisted key
        using (ICryptoTransform encryptor = aes.CreateEncryptor())
        {
            if (!encryptor.CanTransformMultipleBlocks)
            {
                throw new InvalidOperationException("This is a sample, this case wasn’t handled...");
            }
                return encryptor.TransformFinalBlock(data, 0, data.Length);
        }
    }
}
```

### SignedXml Support for SHA-2 Hashing

The .NET Framework 4.6.2 has added support to SignedXml which permits RSA-SHA256, RSA-SHA384, and RSA-SHA512 PKCS#1 signature methods, and SHA256, SHA384, and SHA512 reference digest algorithms.

The URI constants are all exposed on SignedXml:

```csharp
public const string XmlDsigSHA256Url = "http://www.w3.org/2001/04/xmlenc#sha256";
public const string XmlDsigRSASHA256Url = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
public const string XmlDsigSHA384Url = "http://www.w3.org/2001/04/xmldsig-more#sha384";
public const string XmlDsigRSASHA384Url = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384";
public const string XmlDsigSHA512Url = "http://www.w3.org/2001/04/xmlenc#sha512";
public const string XmlDsigRSASHA512Url = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512";

```
Any programs which have registered a custom SignatureDescription handler into CryptoConfig to add support for these algorithms will continue to function as they did in the past, but since there are now platform defaults the CryptoConfig registration should no longer be necessary.


```csharp
private static void SignDocumentRsaSha256(XmlDocument doc, RSA privateKey)
{
    SignedXml signer = new SignedXml(doc);
    Reference reference = new Reference("");
    reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
    reference.DigestMethod = SignedXml.XmlDsigSHA256Url;
    signer.AddReference(reference);
    signer.SigningKey = privateKey;
    signer.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;
    signer.ComputeSignature();

    doc.DocumentElement.AppendChild(signer.GetXml());
} 
```

Windows Presentation Foundation
-----------

### Soft Keyboard Support

Soft Keyboard support enables focus tracking in a WPF applications by automatically invoking and dismissing the new Soft Keyboard in Windows 10 when the touch input is received by a control that can take textual input.
Today, WPF applications cannot opt into the focus tracking without disabling WPF pen/touch gesture support.  As a result, WPF applications must choose between full WPF touch supports or rely on Windows mouse promotion.

###  Per-Monitor DPI Support
WPF applications are [system-DPI aware](https://msdn.microsoft.com/en-us/library/windows/desktop/dn280512%28v=vs.85%29.aspx), which means that applications are scaled by Windows depending on the DPI of the monitor on which the application is being rendered. This can result in loss of sharpness, blurry text etc. Prior to 4.6.2, [additional native code](https://msdn.microsoft.com/en-us/library/windows/desktop/ee308410%28v=vs.85%29.aspx) was required to enable per-monitor DPI awareness in WPF applications.

Given the recent proliferation of high-DPI and hybrid-DPI environments in the ecosystem, we have now enabled per-monitor DPI awareness in WPF applications. See the [samples and developer guide](https://github.com/rohit21agrawal/WPF-Samples/tree/master/PerMonitorDPI) for more information around how to enable you WPF application to become per-monitor DPI aware. 








