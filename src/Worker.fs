module Worker

open System

open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop

open Fetch
open Fetch.Utils
open Fable.Cloudflare.Workers
open Browser

let getAssetsFromKV (ev: FetchEvent) (opts: obj) : Promise<Response> = import "getAssetFromKV" "@cloudflare/kv-asset-handler"
let mapRequestToAsset: Request -> Request = import "mapRequestToAsset" "@cloudflare/kv-asset-handler"


let (|Api|HelloWorld|Static|) (req: Request) =
    let url = URL.Create req.url
    let parms = url.searchParams
    match url.pathname, parms.get "code" with
    | "/api", Some code -> Api (code)
    | "/api", None -> Api "no parameter"
    | "/hello", _ -> HelloWorld
    | _ -> Static

let get404 (req: Request) =
    let url = URL.Create req.url
    url.pathname <- "404.html"
    let url' = url.toString()
    let newReq = newRequestUrl url' req
    mapRequestToAsset newReq



//The worker code is here. Define a request handler which creates an an
// appropreate Response and returns a Promise<Response>
let private handleRequest (e:FetchEvent) =
    promise {
        try
            match e.request with
            | Api input ->

                // YOUR CODE HERE
                let txt = sprintf "Api call with %s" input
                let status : ResponseInit = !! {| status = "200" |}
                let response = newResponse txt status
                return response
            | HelloWorld ->
                let status : ResponseInit = !! {| status = "200" |}
                let response = newResponse "Hello World" status
                return response
            | Static ->
                return! getAssetsFromKV e (createEmpty<obj>)
        with
        | _ ->
            return! getAssetsFromKV e (!!{| mapRequestToAsset = get404 |})

        }


// Register a listner for the ServiceWorker 'fetch' event. That listner
// will extract the request and dispath it to the request handler.
addEventListener_fetch (fun (e:FetchEvent) ->
    e.respondWith (!^ (handleRequest e)))
