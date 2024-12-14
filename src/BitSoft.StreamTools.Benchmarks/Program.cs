using BenchmarkDotNet.Running;
using System.Reflection;

new BenchmarkSwitcher(typeof(Program).GetTypeInfo().Assembly).Run(args);