namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("FSharp.AspNetCore.ServiceWrapper")>]
[<assembly: AssemblyProductAttribute("FSharp.AspNetCore.Examples")>]
[<assembly: AssemblyDescriptionAttribute("Example AspNetCore Project running on Kestrel in F#")>]
[<assembly: AssemblyVersionAttribute("1.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0"
    let [<Literal>] InformationalVersion = "1.0"
