# MondoCore.Security
  Classes for encryption, key rotation, password hashing and time based one-time passwords (TOTP). 
  ###### Important Note: These classes are wrappers around classes in the System.Security.Cryptography namespace and do not actually implement encryption algorithms.
&nbsp;
## Encryption

#### SymmetricEncryptor
Encrypts and decrypts using the Advanced Encryption Standard (AES) algorithm
```
using MondoCore.Security.Encryption;

// Create a new symmetric encryptor with a new key with a default policy
IEncryptor encryptor = new SymmetricEncryptor(new Key(new EncryptionPolicy()));

// Encrypt a string
string originalText = "Bob's your uncle";
string cipherText = await encryptor.Encrypt(originalText);

Console.WriteLine(cipherText);

// Decrypt the encrypted text
string plainText = await encryptor.Decrypt(cipherText);

Console.WriteLine($"\"{plainText}\"" == \"{originalText}\"");
```

#### RotatingKeyEncryptor
An encryptor that generates new encryptors (keys) and rotates (expires) after a certain interval. Expired encryptors can still be used for decrypting but cannot be used for encrypting new data.

```
using MondoCore.Security.Encryption;

// This should be called from your dependency injection code and then the resulting IEncryptor can be used as in the example above. The stores should use secure storage such as Azure KeyVault or AWS Key Management Service. Note; it is important that the encrypt and decrypt stores are separate containers
public static IEncryptor CreateEncryptor(IBlobStore encryptStore, 
                                         IBlobStore decryptStore, 
                                         IEncryptor kek)
{
    var encryptorCache = new MemoryCache();
    
    var keyFactory = new KeyFactory(new KeyStore(decryptStore, kek),
                                    new KeyStore(encryptStore, kek),
                                    new EncryptionPolicy(), 
                                    new TimeSpan(90, 0, 0, 0)); // Expires after 90 days

    return new RotatingKeyEncryptor(new SymmetricEncryptorFactory(encryptorCache, keyFactory));
}
```

## Password Management

### PasswordManager
Manages loading, saving and validating passwords
```
using MondoCore.Security.Encryption;
using MondoCore.Security.Password;

public Password ValidatePassword(string password, // The password entered by the user
                                 IPasswordOwner owner // Usually a guid (from NoSql): Use GuidPasswordOwner
                                                      //   or an int/long (SQL) that identifies the user/owner
                                                      //       Use LongPasswordOwner
                                )
{
    // To encrypt the salt. Should be persisted
    IEncryptor encryptor = new SymmetricEncryptor(new Key(new EncryptionPolicy())); 
    
    // See https://en.wikipedia.org/wiki/PBKDF2 for hash iteration recommendations
    IPasswordHasher hasher = new PasswordHasher(11528); 
 
    // You need to implement this interface. This is usually a SQL or NoSql database. 
    // It can be your user/member/staff database but it is recommended to store passwords in a different database and server
    IPasswordStore passwordStore = new MyPasswordStore();
 
    IPasswordManager passwordManager = new PasswordManager(hasher, passwordStore, encryptor);
 
    using(Password enteredPassword = passwordManager.FromOwner(password, owner))
    {
        using(Password storedPassword = passwordManage.Load(owner))
        {
            return enteredPassword.IsEqual(storedPassword);
        }
    }
}
```
License
----

MIT
