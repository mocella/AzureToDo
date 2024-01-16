// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AzureToDo.Tests.Api;

[Trait(Constants.TestCategory, Constants.IntegrationTestCategory)]
public class IntegrationTestVerification
{
    [Fact]
    public void DemoFail()
    {
        Assert.Equal(1,2);
    }
}
