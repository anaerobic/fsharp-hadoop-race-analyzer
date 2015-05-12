module Controller

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
    
    // Define the input sequence
    let rec inputs() = 
        seq { 
            let input = readLine()
            if not (String.IsNullOrWhiteSpace(input)) then 
                // Yield the input and the remainder of the sequence
                yield input
                yield! inputs()
        }
    inputs()
    |> Seq.map Mapper.Map
    |> Seq.filter Option.isSome
    |> Seq.iter (fun value -> outputCollector value.Value)
