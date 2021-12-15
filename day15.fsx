open System
open System.IO
open System.Collections.Generic

let data = 
    let raw = File.ReadAllLines("data/day15")
    Array2D.init raw.Length raw.Length (fun x y -> (string >> Int32.Parse) raw.[x].[y])

let adjacent x y (grid: int[,]) = seq {
    if x < Array2D.length1 grid - 1 then yield x + 1, y
    if y < Array2D.length2 grid - 1 then yield x, y + 1
    if x > 0 then yield x - 1, y
    if y > 0 then yield x, y - 1
}

let findPath (data: int[,]) =
    let queue = PriorityQueue<int * int * int, int>()
    let visited = Dictionary<int * int, int>()
    queue.Enqueue((0, 0, 0), 0)

    let rec find () =
        let x, y, pathSoFar = queue.Dequeue()
        if x = Array2D.length1 data - 1 && y = Array2D.length2 data - 1 then pathSoFar
        else
            for x, y in adjacent x y data do
                let newLen = pathSoFar + data.[x, y]
                if not (visited.ContainsKey((x, y))) || newLen < visited.[(x, y)] then
                    visited.[(x, y)] <- newLen
                    queue.Enqueue((x, y, newLen), newLen)
            find ()
    find ()

let expand (a: int[,])= 
    Array2D.init (Array2D.length1 a * 5) (Array2D.length2 a * 5) (fun x y -> 
        let value = a.[x % Array2D.length1 a, y % Array2D.length2 a] + x / Array2D.length1 a + y / Array2D.length2 a
        if value > 9 then value % 10 + 1 else value
    )

findPath data |> printfn "%d"
expand data |> findPath |> printfn "%d"