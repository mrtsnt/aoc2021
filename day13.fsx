open System
open System.IO
open System.Text.RegularExpressions

let data = File.ReadAllText("data/day13")

let grid =
    let marks = 
        data.Split("\n\n").[0].Split("\n")
        |> Array.map (fun r -> Int32.Parse (r.Split(',').[0]), Int32.Parse (r.Split(',').[1]))
    let grid = Array2D.create (Array.maxBy fst marks |> fst |> (+) 1) (Array.maxBy snd marks |> snd |> (+) 1) 0
    Array.iter (fun (x, y) -> grid.[x, y] <- 1) marks
    grid

let folds = 
    data.Split("\n\n").[1].Split("\n")
    |> Array.map (fun r -> 
        let m = [for x in Regex.Match(r, @"([xy])=(\d+)").Groups -> x.ToString()]
        m.[1], Int32.Parse m.[2])

let fold grid folds =
    Array.fold (fun (s: int[,]) (d, p) ->
        if d = "x" then 
            Array2D.iteri (fun x y v -> s.[p - 1 - x, y] <- s.[p - 1 - x, y] ||| v) s.[p + 1.., *]
            s.[0..(p - 1), *]
        else
            Array2D.iteri (fun x y v -> s.[x, p - 1 - y] <- s.[x, p - 1 - y] ||| v) s.[*, p + 1..]
            s.[*, 0..(p - 1)]
    ) (Array2D.copy grid) folds

let mutable sum = 0
Array2D.iter (fun v -> sum <- sum + v) (fold grid [|folds.[0]|])
printfn "%d" sum

let folded = fold grid folds
for y in [0..Array2D.length2 folded - 1] do
    Array.iter(fun v -> if v = 1 then printf "#" else printf " ") folded.[*, y]
    printf "\n"