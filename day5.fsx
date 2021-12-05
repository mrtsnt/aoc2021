open System
open System.IO

let getData () =
    File.ReadAllLines("data/day5")
    |> Array.map (fun r ->
        let split = r.Split(" -> ")
        split.[0].Split(",").[0] |> Int32.Parse,
        split.[0].Split(",").[1] |> Int32.Parse,
        split.[1].Split(",").[0] |> Int32.Parse,
        split.[1].Split(",").[1] |> Int32.Parse)

let tryMarkHorizontal x y x' y' (grid: int[,]) =
    if x = x' then [min y y'..max y y'] |> List.iter (fun y -> grid.[y, x] <- grid.[y, x] + 1)

let tryMarkVertical x y x' y' (grid: int[,]) =
    if y = y' then [min x x'..max x x'] |> List.iter (fun x -> grid.[y, x] <- grid.[y, x] + 1)

let tryMarkDiagonal x y x' y' (grid: int[,]) =
    if abs (x - x') = abs (y - y') then
        let isAsc = if x < x' then y < y' else y > y'
        let yTransform = if isAsc then id else List.rev
        List.zip [min x x'..max x x'] (yTransform [min y y'..max y y']) |> List.iter (fun (x, y) -> grid.[y, x] <- grid.[y, x] + 1)

let solve marks (data: (int * int * int * int)[]) = 
    let grid = Array2D.zeroCreate<int> 1000 1000
    Array.iter(fun (x, y, x', y') -> for mark in marks do mark x y x' y' grid) data

    let mutable cnt = 0
    Array2D.iter (fun v -> if v >= 2 then cnt <- cnt + 1) grid
    cnt

let data = getData ()
solve [tryMarkHorizontal; tryMarkVertical] data |> printfn "part1 %d"
solve [tryMarkHorizontal; tryMarkVertical; tryMarkDiagonal] data |> printfn "part2 %d"