# MondoCore.Common
  General purpose classes and interfaces used by other MondoCore libraries. 

## Interfaces

### IBlobStore

Interface for accessing blobs/files.

#### Implementations

* FileStore
* MemoryStore
* AzureStorage (in MondoCore.Azure.Storage)

<br>

### ICache

Interface for caching data.

#### Implementations

* MemoryCache


<br>


### IMessageQueue

Interface for sending messages

#### Implementations

* AzureStorageQueue (in MondoCore.Azure.Storage.Queue)

<br>


```
License
----

MIT
