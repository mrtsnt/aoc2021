open System
open System.IO
open System.Collections.Generic

type Token 
    = Open 
    | Close 
    | Num of int64

let toTokens (str: string) = 
    str.Replace("[", " [ ").Replace("]", " ] ").Replace(",", " ").Split(([||]: char[]), StringSplitOptions.RemoveEmptyEntries)
    |> List.ofArray
    |> List.map (function
        | "[" -> Open
        | "]" -> Close
        | n -> Num(Int64.Parse(n)))

let rec addNum ls toAdd =
    match ls with
    | [] -> []
    | Num(n)::xs -> (Num(n + toAdd))::xs
    | x::xs -> x::(addNum xs toAdd)

let explode (tokens: Token list) =
    let rec explode' soFar tokens nest = 
        match nest, tokens with
        | 4, Open::Num(n1)::Num(n2)::Close::rest -> 
            List.rev (addNum soFar n1) @ [Num(0)] @ addNum rest n2
        | _, x::xs ->
            match x with
            | Open -> explode' (x::soFar) xs (nest + 1)
            | Close -> explode' (x::soFar) xs (nest - 1)
            | _ -> explode' (x::soFar) xs nest
        | _, [] -> List.rev soFar

    explode' [] tokens 0

let rec split = function
    | [] -> []
    | (Num n)::xs when n > 9 -> 
        let left = n / 2L
        let right = Math.Ceiling(decimal n / 2m) |> int
        Open::Num(left)::Num(right)::Close::xs
    | x::xs -> x::(split xs)

let add l1 l2 =
    let combined = [Open] @ l1 @ l2 @ [Close]
    let rec reduce ls =
        let afterExplosion = explode ls
        if afterExplosion = ls then
            let afterSplit = split ls
            if afterSplit = ls then ls
            else reduce afterSplit
        else reduce afterExplosion

    reduce combined

type Tree
    = Number of int64
    | Branch of Tree * Tree

let parse (tokens: Token list) =
    let st = List.rev tokens |> Stack<Token>

    let rec parse' () =
        let token = st.Pop()
        match token with
        | Open ->
            let left = parse' ()
            let right = parse' ()
            st.Pop() |> ignore
            Branch(left, right)
        | Num n -> Number(n)
        | _ -> failwith "unexpected ']'"

    parse' ()

let rec magnitude = function
    | Number(n) -> n
    | Branch(l, r) -> magnitude l * 3L + magnitude r * 2L

let data = File.ReadAllLines("data/day18")

let part1 data = 
    Array.map toTokens data |> Array.reduce add |> parse |> magnitude

let part2 data =
    let tokenized = Array.map toTokens data |> List.ofArray
    tokenized
    |> List.collect (fun t -> List.collect (fun tt -> [(t, tt); (tt, t)]) tokenized)
    |> List.map (fun (a, b) -> add a b |> parse |> magnitude)
    |> List.max

part1 data |> printfn "%d"
part2 data |> printfn "%d"