using PW.FluxQueryNet.Options;
using System;

namespace PW.FluxQueryNet
{
    public interface IFluxConfigurable
    {
        /// <summary>
        /// Adjust <see cref="FluxBuilderOptions"/> configuration while building the Flux query.
        /// </summary>
        IFluxStream Configure(Action<FluxBuilderOptions> configureAction);
    }
}
