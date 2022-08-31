module Action

open Fable.Core
open Fable.Core.JsInterop

let [<Global>] console: JS.Console = jsNative

[<Import("*", from="@actions/core")>]
let core : ActionsCore.Core.IExports = jsNative

[<Import("*", from="@actions/github")>]
let github : ActionsGithub.Github.IExports = jsNative

try
  let nameToGreet = core.getInput "who-to-greet"
  console.log $"Hello {nameToGreet}"
  let time = (createNew JS.Constructors.Date ())?toTimeString()
  core.setOutput("time", Some time)
  // Get the JSON webhook payload for the event that triggered the workflow
  let payload = JS.JSON.stringify(value = github.context.payload, space = 2)
  console.log $"The event payload: {payload}"
with
| ex -> core.setFailed(U2.Case1 ex.Message)
