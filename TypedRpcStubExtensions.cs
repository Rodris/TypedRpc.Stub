using Owin;
using TypedRpc;
using TypedRpc.Client;

namespace TypedRpcStub
{
    // Extensions
    public static class TypedRpcStubExtensions
    {
        /// <summary>
        /// Maps RpcServerStub in OWIN.
        /// </summary>
        public static void MapTypedRpcStub(this IAppBuilder app, TypedRpcOptions options = null)
        {
			app.Map("/typedrpc/client", appBuilder => appBuilder.Use<TypedRpcClientMiddleware>(new object[0]));
			app.Map("/typedrpc", appBuilder => appBuilder.Use<TypedRpcStubMiddleware>(new object[] { options }));
		}
	}

}