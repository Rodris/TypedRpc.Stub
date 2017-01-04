# TypedRpc.Stub

Stub functionality for the [TypedRpc](https://github.com/Rodris/TypedRpc) library.

This library enables the client side of an application to be developed independently from the server side.

## Installation

To install from NuGet, run the following command in the Package Manager Console:

```
Install-Package TypedRpc.Stub
```

## Usage

This library uses the yaml format to specify possible method calls and their returns.

The stubs are defined in the file TypedRpc / TypedRpcStubs.yaml

Changes to the yaml file take effect instantly. No need to restart the server.

Each stub is a record in a list. The fields are:
- Name: must be formed by the name of the handler, period, name of the method.
- Parameters: list with parameter values ​​that will be received.
- Return: the return value if the method executes successfully.
- Error: of type JsonError, if the method returns an error.

### Example

The following example prepares two stubs.
The first invokes the `HelloWorld` method of the `MyHandler` handler and receives a string as a return.
The second invokes the `SaveData` method of the same handler and returns an error.

```yaml
- Name: MyHandler.HelloWorld
  Return: Hello, world!

- Name: MyHandler.SaveData
  Parameters:
  - Id: 123
  - Desc: Data to be saved.
  Error:
    Code: 12
    Message: Stub for error.
    Data: Extended error description.
```

## License

The contents of this repository are covered under the [Mit License](http://opensource.org/licenses/MIT).
