module Program

open Controller
open ParseCommandLine

[<EntryPoint>]
let main argv = 
    let args = parseArgs <| Array.toList argv <| Args defaults
    match args with
    | ParseError message -> 
        handleInvalidArgs message
        1
    | Args options -> 
        Controller.Run options
        0
