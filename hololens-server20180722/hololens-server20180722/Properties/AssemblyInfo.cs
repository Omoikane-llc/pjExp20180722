﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// アセンブリに関する一般情報は、以下を通して制御されます
// 情報を COM コンポーネントに変更するには、これらの属性の値を変更してください。
// これらの属性の値を変更します。
[assembly: AssemblyTitle("hololens_server20180722")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("hololens_server20180722")]
[assembly: AssemblyCopyright("Copyright ©  2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// ComVisible を false に設定すると、アセンブリ内のタイプが非表示になります
// 次の場所からこのアセンブリ内の型にアクセスする必要がある場合、
//COM、その型の ComVisible 属性を true に設定します。
[assembly: ComVisible(false)]

// このプロジェクトが COM 属性セットに公開される場合、次の GUID は typelib の ID 用です。
[assembly: Guid("3ba76695-9419-4611-aca4-4aa6c5bafef9")]

// アセンブリのバージョン情報は、次の 4 つの値で構成されます:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// すべての値を指定するか、リビジョンおよびビルド番号を既定値にできます 
// 以下に示すように '*' を使用します:
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
// ログアセンブリ
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]
