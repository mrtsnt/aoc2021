open System
open System.IO

let data =
    let raw = File.ReadAllLines("data/day9")
    Array2D.init 100 100 (fun x y -> (string >> Int32.Parse) raw.[x].[y])

let getAdjacent r c (data: int[,]) =
    seq {
        if r <> 0 then yield r - 1, c, data.[r - 1, c]
        if r <> 99 then yield r + 1, c, data.[r + 1, c]
        if c <> 0 then yield r, c - 1, data.[r, c - 1]
        if c <> 99 then yield r, c + 1, data.[r, c + 1]
    }

let part1 (data: int[,]) =
    let mutable totalRisk = 0
    Array2D.iteri (fun r c v ->
        if getAdjacent r c data |> Seq.forall (fun (_, _, v') -> v < v') then
            totalRisk <- v + 1 + totalRisk
    ) data
    totalRisk

let part2 (data: int[,]) =
    let mutable lowPoints = []
    Array2D.iteri (fun r c v ->
        if getAdjacent r c data |> Seq.forall (fun (_, _, v') -> v < v') then
            lowPoints <- (r, c)::lowPoints
    ) data

    let getBasinSize (r, c) =
        let mutable visited = Set.empty
        let rec visit r c = 
            for (r, c, v) in getAdjacent r c data do
                if v <> 9 && not (Set.contains (r, c) visited) then
                    visited <- Set.add (r, c) visited
                    visit r c
        visit r c
        Set.count visited

    lowPoints |> List.map getBasinSize |> List.sortDescending |> Seq.take 3 |> Seq.reduce (*)

part1 data |> printfn "%d"
part2 data |> printfn "%d"