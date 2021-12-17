#r "nuget: FSharp.Collections.ParallelSeq"
open FSharp.Collections.ParallelSeq

let xMin, xMax, yMin, yMax = 124, 174, -123, -86
let inRange (x, y) = x <= xMax && y >= yMin
let inTarget (x, y) = inRange (x, y) &&  x >= xMin && y <= yMax

let points xv yv = 
    let rec impl x y (xv, yv) =
        seq {
            yield (x, y)
            yield! impl (x + xv) (y + yv) ((if xv - 1 < 0 then 0 else xv - 1), (yv - 1))
        }
    impl 0 0 (xv, yv)

let generateRange x x' y y' = Seq.collect(fun x -> [y..y'] |> Seq.map(fun y -> (x, y))) [x..x']

let part1 () =
    generateRange 1 1000 1 1000
    |> PSeq.map (fun (xv, yv) ->
        let allPoints = points xv yv |> Seq.takeWhile inRange |> Array.ofSeq
        if inTarget <| Array.last allPoints then Array.maxBy snd allPoints |> snd else 0)
    |> PSeq.max

let part2 () =
    generateRange 1 1000 -1000 1000
    |> PSeq.filter (fun (xv, yv) -> points xv yv |> Seq.takeWhile inRange |> Seq.last |> inTarget)
    |> PSeq.length

part1 () |> printfn "%d"
part2 () |> printfn "%d"