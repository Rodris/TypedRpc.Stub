using JsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypedRpc.Stub
{
    /// <summary>
    /// Fake method used by middleware to handle RPCs.
    /// </summary>
    public class MethodStub
    {
        /// <summary>
        /// Method name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Method parameters.
        /// </summary>
        public object[] Parameters { get; set; }

        /// <summary>
        /// Method return.
        /// </summary>
        public object Return { get; set; }

        /// <summary>
        /// Error when calling this method.
        /// </summary>
        public JsonError Error { get; set; }
    }
}
