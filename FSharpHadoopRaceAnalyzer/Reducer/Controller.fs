﻿module Controller

open InputOutput
open ParseCommandLine
open System

let Run options = 
    // Ensure Standard Input/Output and allow for debug configuration
    use reader = getReader options.input
    use writer = getWriter options.output
    
    // Combine the name/value output into a string
    let outputCollector keyValue = 
        let output = getOutput keyValue
        writer.WriteLine(output)
    
    // Read the next line of the input stream
    let readLine() = reader.ReadLine()
    
    // Parse the input into the required name/value pair
    let parseLine (input : string) = 
        let keyValue = input.Split('\t')
        (keyValue.[0].Trim(), keyValue.[1].Trim())
    
    // Converts a input line into an option
    let getInput() = 
        let input = readLine()
        if not (String.IsNullOrWhiteSpace(input)) then Some(parseLine input)
        else None
    
    // Creates a sequence of the input based on the provided key
    let lastInput = ref None
    let continueDo = ref false
    
    let inputsByKey key firstValue = 
        seq { 
            // Yield any value from previous read
            yield firstValue
            continueDo := true
            while !continueDo do
                match getInput() with
                | Some(input) when (fst input) = key -> 
                    // Yield found value and remainder of sequence
                    yield (snd input)
                | Some(input) -> 
                    // Have a value but different key
                    lastInput := Some(fst input, snd input)
                    continueDo := false
                | None -> 
                    // Have no more entries
                    lastInput := None
                    continueDo := false
        }
    
    // Controls the calling of the reducer
    let rec processInput (input : (string * string) option) = 
        if input.IsSome then 
            let key = fst input.Value
            let value = Reducer.Reduce key (inputsByKey key (snd input.Value))
            if value.IsSome then outputCollector (key, value.Value)
            processInput lastInput.contents
    
    processInput (getInput())