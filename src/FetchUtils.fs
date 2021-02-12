module Fetch.Utils

open Fetch
open Fable.Core

[<Emit("new Response($0, $1)")>]
let newResponse (a: string) (b:ResponseInit) : Response = jsNative

[<Emit("new Request($0)")>]
let newRequest (b: RequestInit): Request =  jsNative

[<Emit("new Request($0, $1)")>]
let newRequestUrl (url: string) (b: Request) : Request = jsNative

[<Emit("Response.redirect($0, $1)")>]
let responseRedirect (url: string) (code: int) : Response = jsNative