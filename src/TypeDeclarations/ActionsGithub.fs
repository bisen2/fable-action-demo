// ts2fable 0.7.1
module rec ActionsGithub
open System
open Fable.Core
open Fable.Core.JS

module Context =
    type WebhookPayload = Interfaces.WebhookPayload

    type [<AllowNullLiteral>] IExports =
        abstract Context: ContextStatic

    type [<AllowNullLiteral>] Context =
        /// Webhook payload object that triggered the workflow
        abstract payload: WebhookPayload with get, set
        abstract eventName: string with get, set
        abstract sha: string with get, set
        abstract ref: string with get, set
        abstract workflow: string with get, set
        abstract action: string with get, set
        abstract actor: string with get, set
        abstract job: string with get, set
        abstract runNumber: float with get, set
        abstract runId: float with get, set
        abstract apiUrl: string with get, set
        abstract serverUrl: string with get, set
        abstract graphqlUrl: string with get, set
        abstract issue: {| owner: string; repo: string; number: float |}
        abstract repo: {| owner: string; repo: string |}

    type [<AllowNullLiteral>] ContextStatic =
        /// Hydrate the context from the environment
        [<Emit "new $0($1...)">] abstract Create: unit -> Context

module Github =
    // module Context = __context
    type GitHub = obj // TODO Utils.GitHub
    type OctokitOptions = obj // TODO __@octokit_core_dist_types_types.OctokitOptions

    type [<AllowNullLiteral>] IExports =
        abstract context: Context.Context
        /// <summary>Returns a hydrated octokit ready to use for GitHub Actions</summary>
        /// <param name="token">the repo PAT or GITHUB_TOKEN</param>
        /// <param name="options">other options to set</param>
        abstract getOctokit: token: string * ?options: OctokitOptions -> obj // TODO InstanceType<obj>

module Interfaces =

    type [<AllowNullLiteral>] PayloadRepository =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option with get, set
        abstract full_name: string option with get, set
        abstract name: string with get, set
        abstract owner: PayloadRepositoryOwner with get, set
        abstract html_url: string option with get, set

    type [<AllowNullLiteral>] WebhookPayload =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option with get, set
        abstract repository: PayloadRepository option with get, set
        abstract issue: WebhookPayloadIssue option with get, set
        abstract pull_request: WebhookPayloadIssue option with get, set
        abstract sender: WebhookPayloadSender option with get, set
        abstract action: string option with get, set
        abstract installation: WebhookPayloadInstallation option with get, set
        abstract comment: WebhookPayloadInstallation option with get, set

    type [<AllowNullLiteral>] PayloadRepositoryOwner =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option with get, set
        abstract login: string with get, set
        abstract name: string option with get, set

    type [<AllowNullLiteral>] WebhookPayloadIssue =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option with get, set
        abstract number: float with get, set
        abstract html_url: string option with get, set
        abstract body: string option with get, set

    type [<AllowNullLiteral>] WebhookPayloadSender =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option with get, set
        abstract ``type``: string with get, set

    type [<AllowNullLiteral>] WebhookPayloadInstallation =
        abstract id: float with get, set
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option with get, set

module Utils =
    // module Context = __context
    type Octokit = obj // TODO __@octokit_core.Octokit
    type OctokitOptions = obj // TODO __@octokit_core_dist_types_types.OctokitOptions

    type [<AllowNullLiteral>] IExports =
        abstract context: Context.Context
        abstract GitHub: obj
        /// <summary>Convience function to correctly format Octokit Options to pass into the constructor.</summary>
        /// <param name="token">the repo PAT or GITHUB_TOKEN</param>
        /// <param name="options">other options to set</param>
        abstract getOctokitOptions: token: string * ?options: OctokitOptions -> OctokitOptions

module __internal_utils =
    type OctokitOptions = obj // TODO __internal_@octokit_core_dist_types_types.OctokitOptions

    type [<AllowNullLiteral>] IExports =
        abstract getAuthString: token: string * options: OctokitOptions -> string option
        abstract getProxyAgent: destinationUrl: string -> obj // TODO Http.Agent
        abstract getApiBaseUrl: unit -> string
