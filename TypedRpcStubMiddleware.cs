using JsonRpc;
using Microsoft.Owin;
using System;
using System.Linq;
using System.IO;
using TypedRpc;
using YamlDotNet.Serialization;
using System.Reflection;

namespace TypedRpcStub
{
    // Server main class.
    public class TypedRpcStubMiddleware : TypedRpcMiddleware
    {
        // Available methods stubs.
        private MethodStub[] MethodStubs;

        // Last time stubs were updated.
        private DateTime LastUpdateTime;

        // Constructor
        public TypedRpcStubMiddleware(OwinMiddleware next, TypedRpcOptions options)
            : base(next, options)
        {
            // Initializations
            Init();
        }

        // Initializes stub.
        private void Init()
        {
            // Loads stub data.
            LoadData();
        }

        // Invokes a method.
        protected override object InvokeMethod(object handler, MethodInfo methodInfo, object[] parameters)
        {
            // Declarations
            MethodStub methodStub;
            string methodName;

            // Removes Owin context.
            parameters = parameters.Where(p => !(p is IOwinContext)).ToArray();

            // Loads stub data.
            LoadData();

            // Mounts method name.
            methodName = String.Format("{0}.{1}", handler.GetType().Name, methodInfo.Name);

            // Searches for a matched stub.
            methodStub = MethodStubs.SingleOrDefault(ms => ms.Name == methodName &&
                EqualsParameters(ms.Parameters, parameters));

            // Checks if stub was not found.
            return (methodStub == null) ? MountStubError(methodName, parameters) :
                (methodStub.Return != null) ? methodStub.Return : methodStub.Error;
        }
        
        // Mounts a stub error.
        private JsonError MountStubError(string methodName, object[] parameters)
        {
            // Declarations
            MethodStub methodStub;
            
            // Mounts requested stub.
            methodStub = new MethodStub()
            {
                Name = methodName,
                Parameters = (parameters != null && parameters.Length > 0) ? parameters : null
            };
            
            // Mounts error for request.
            return new JsonError()
            {
                Code = 1,
                Message = "Method stub not found.",
                Data = new Serializer().Serialize(new MethodStub[] { methodStub })
            };
        }

        // Compares two sets of parameters.
        private bool EqualsParameters(object[] stubParams, object[] requestParams)
        {
            // Checks for empty parameters.
            if ((stubParams == null || stubParams.Length == 0) &&
                (requestParams == null || requestParams.Length == 0))
            {
                return true;
            }

            // Compares serialized parameters.
            return JsonSerializer.Serialize(stubParams) == JsonSerializer.Serialize(requestParams);
        }

        // Loads data.
        private void LoadData()
        {
            // Declarations
            DateTime fileWriteTime;
            string filePath;
            string data;

            // Safe code.
            lock (this)
            {
                // Initializations
                filePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\TypedRpc\\TypedRpcStubs.yaml";

                // Checks if stubs file exists.
                if (!File.Exists(filePath)) return;

                // Retrieves file last write time.
                fileWriteTime = File.GetLastWriteTime(filePath);

                // Checks if file was updated.
                if (fileWriteTime > LastUpdateTime)
                {
                    // Loads data.
                    data = File.ReadAllText(filePath);
                    
                    // Parses data.
                    MethodStubs = new Deserializer().Deserialize<MethodStub[]>(data);

                    // Stubs updated.
                    LastUpdateTime = fileWriteTime;
                }
            }
        }
        
    }
}