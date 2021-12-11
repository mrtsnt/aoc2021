open System 
open System.IO

let adjacent x y =
    let inRange x y = x >= 0 && x <= 9 && y >= 0 && y <= 9 
    seq {
        for x' in [x - 1..x + 1] do
            for y' in [y - 1..y + 1] do
                if inRange x' y' && (x' <> x || y' <> y) then yield x', y'
    }

let rec tryFlash x y (grid: int option[,]) =
    match grid.[x, y] with
    | Some n ->
        if n > 9 then
            grid.[x, y] <- None
            for x', y' in adjacent x y do
                grid.[x', y'] <- Option.map (fun n -> n + 1) grid.[x', y']
                tryFlash x' y' grid
    | None -> ()

let passDay grid =
    let increased = Array2D.map (Option.map (fun n -> n + 1)) grid
    Array2D.iteri (fun x y _ -> tryFlash x y increased) increased
    Array2D.map (fun v -> match v with None -> Some 0 | _ -> v) increased

let countFlashes grid = 
    let mutable c = 0
    Array2D.iter (fun v -> if v = Some 0 then c <- c + 1) grid
    c

let data =
    let lines = File.ReadAllLines("data/day11")
    Array2D.init 10 10 (fun x y -> (string >> Int32.Parse >> Some) lines.[x].[y])

let part1 (grid: int option[,]) =
    List.fold (fun (g, c) _ -> 
        let newGrid = passDay g
        newGrid, countFlashes newGrid + c
    ) (grid, 0) [1..100] |> snd

let part2 (grid: int option[,]) =
    let rec dayRoll grid = seq {
        let newGrid = passDay grid
        yield countFlashes grid
        yield! dayRoll newGrid
    }
    Seq.findIndex (fun f -> f = 100) (dayRoll grid)

part1 data |> printfn "%d"
part2 data |> printfn "%d"