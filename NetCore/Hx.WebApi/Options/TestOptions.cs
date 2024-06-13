// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.Options;

namespace Hx.WebApi.Options;

public class TestOptions: IConfigureOptions<TestOptions>
{
    public string Name { get; set; }

    public void Configure(TestOptions options)
    {
        options.Name = "Init";
    }
}
