﻿using Flandre.Core.Common;
using Flandre.Core.Messaging;
using Flandre.Core.Attributes;

// ReSharper disable StringLiteralTypo

namespace Flandre.Core.Tests;

[Plugin("Test")]
public class TestPlugin : Plugin
{
    public override void OnMessageReceived(MessageContext ctx)
    {
        if (ctx.Message.GetText().StartsWith("OMR:"))
            ctx.Bot.SendMessage(ctx.Message);
    }

    [Command("test1 <arg1:bool>")]
    [Option("opt", "-o <opt:double>")]
    [Option("boolopt", "-b <:bool>")]
    [Option("trueopt", "-t <:bool=true>")]
    [Shortcut("测试")]
    [Alias("..test111.11..45...14...")] // test normalize
    public static MessageContent OnTest1(MessageContext ctx, ParsedArgs args)
    {
        var arg1 = args.GetArgument<bool>("arg1");
        var opt = args.GetOption<double>("opt");
        var boolOpt = args.GetOption<bool>("boolopt");
        var trueOpt = args.GetOption<bool>("trueopt");
        return $"arg1: {arg1} opt: {opt} b: {boolOpt} t: {trueOpt}";
    }

    [Command("sub.test")]
    [Alias("sssuuubbb")]
    [Shortcut("子测试")]
    public static MessageContent? OnSubTest(MessageContext ctx, ParsedArgs args) => null;

    [Command("...sub....sub..sub......test..")]
    public static MessageContent? OnSubSubSubTest(MessageContext ctx, ParsedArgs args) => null;
}