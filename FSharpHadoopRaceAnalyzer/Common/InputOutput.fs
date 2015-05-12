module InputOutput

open System.IO
open System

let getReader (input:string) = 
    match input.Length with
        | 0 -> new StreamReader(Console.OpenStandardInput())
        | _ -> new StreamReader(Path.GetFullPath(input))

let getWriter (output:string) =
    match output.Length with
        | 0 -> new StreamWriter(Console.OpenStandardOutput(), AutoFlush = true)
        | _ -> new StreamWriter(Path.GetFullPath(output))
        
let getOutput (outputKey, outputValue) = 
    sprintf "%s\t%O" outputKey outputValue