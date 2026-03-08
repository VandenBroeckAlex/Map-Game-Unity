using UnityEngine;
using NUnit.Framework;

public class BootstrapLoader
{
    [Test]
    public void BootstrapLoaderTest()
    {
        LoaderBootstrap lbs = new LoaderBootstrap();

        lbs.InitializeSimulation();

     }
}
