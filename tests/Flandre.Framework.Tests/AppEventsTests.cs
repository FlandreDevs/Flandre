﻿using Flandre.Adapters.Mock;
using Flandre.Framework.Attributes;

namespace Flandre.Framework.Tests;

public class AppEventsTests
{
    [Fact]
    public void TestEvents()
    {
        var adapter = new MockAdapter();
        var client = adapter.GetChannelClient();

        var app = new FlandreAppBuilder()
            .UseAdapter(adapter)
            .UsePlugin<TestPlugin>()
            .Build();

        var count = 0;
        CommandAttribute? cmdInfo = null;
        Exception? ex = null;

        app.OnStarting += (_, _) => count += 1;
        app.OnReady += (_, _) =>
        {
            count += 10;

            client.SendForReply("throw-ex").GetAwaiter().GetResult();

            app.Stop();
        };
        app.OnStopped += (_, _) => count += 100;

        app.OnCommandInvoking += (_, e) => cmdInfo = e.Command.CommandInfo;
        app.OnCommandInvoked += (_, e) => { ex = e.Exception; };

        app.Start();

        Assert.Equal(111, count);
        Assert.Equal("throw-ex", cmdInfo?.Command);
        Assert.Equal("Test Exception", ex?.Message);
    }
}