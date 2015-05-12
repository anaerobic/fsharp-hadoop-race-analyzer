module ParseCommandLine

open System

type args = 
    { input: string
      output: string }

let defaults = 
    { input = "";
      output = "" }

type parseResult = 
    | Args of args
    | ParseError of string

let rec parseArgs (lst : string list) (defaults : parseResult) : parseResult = 
    match defaults with
    | ParseError message -> defaults
    | Args prev -> 
        match lst with
        | "-input" :: tail -> parseArgs tail.Tail <| Args { prev with input = tail.Head }
        | "-output" :: tail -> parseArgs tail.Tail <| Args { prev with output = tail.Head }
        | [] -> defaults
        | _ -> ParseError("Invalid parameter: " + lst.Head)

let usage = """
Usage: HadoopRaceAnalyzer [-input <path>] [-output <path>]

-input <path> - DFS input file(s) for the Map step
-output <path> - DFS output directory for the Reduce step
            """

let handleInvalidArgs (message : string) = 
    Console.WriteLine message
    Console.WriteLine usage
    match Environment.UserInteractive with
    | true -> 
        Console.WriteLine "Press any key to exit..."
        Console.ReadKey() |> ignore
    | false -> 0 |> ignore
