// ts2fable 0.7.1
module rec ActionsCore
open System
open Fable.Core
open Fable.Core.JS

type Error = System.Exception


module Command =

    type [<AllowNullLiteral>] IExports =
        /// Commands
        /// 
        /// Command Format:
        ///    ::name key=value,key=value::message
        /// 
        /// Examples:
        ///    ::warning::This is the message
        ///    ::set-env name=MY_VAR::some value
        abstract issueCommand: command: string * properties: CommandProperties * message: obj option -> unit
        abstract issue: name: string * ?message: string -> unit

    type [<AllowNullLiteral>] CommandProperties =
        [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option with get, set

module Core =

    type [<AllowNullLiteral>] IExports =
        /// <summary>Sets env variable for this action and future actions in the job</summary>
        /// <param name="name">the name of the variable to set</param>
        /// <param name="val">the value of the variable. Non-string values will be converted to a string via JSON.stringify</param>
        abstract exportVariable: name: string * ``val``: obj option -> unit
        /// <summary>Registers a secret which will get masked from logs</summary>
        /// <param name="secret">value of the secret</param>
        abstract setSecret: secret: string -> unit
        /// Prepends inputPath to the PATH (for this action and future actions)
        abstract addPath: inputPath: string -> unit
        /// <summary>Gets the value of an input.
        /// Unless trimWhitespace is set to false in InputOptions, the value is also trimmed.
        /// Returns an empty string if the value is not defined.</summary>
        /// <param name="name">name of the input to get</param>
        /// <param name="options">optional. See InputOptions.</param>
        abstract getInput: name: string * ?options: InputOptions -> string
        /// <summary>Gets the values of an multiline input.  Each value is also trimmed.</summary>
        /// <param name="name">name of the input to get</param>
        /// <param name="options">optional. See InputOptions.</param>
        abstract getMultilineInput: name: string * ?options: InputOptions -> ResizeArray<string>
        /// <summary>Gets the input value of the boolean type in the YAML 1.2 "core schema" specification.
        /// Support boolean input list: `true | True | TRUE | false | False | FALSE` .
        /// The return value is also in boolean type.
        /// ref: https://yaml.org/spec/1.2/spec.html#id2804923</summary>
        /// <param name="name">name of the input to get</param>
        /// <param name="options">optional. See InputOptions.</param>
        abstract getBooleanInput: name: string * ?options: InputOptions -> bool
        /// <summary>Sets the value of an output.</summary>
        /// <param name="name">name of the output to set</param>
        /// <param name="value">value to store. Non-string values will be converted to a string via JSON.stringify</param>
        abstract setOutput: name: string * value: obj option -> unit
        /// Enables or disables the echoing of commands into stdout for the rest of the step.
        /// Echoing is disabled by default if ACTIONS_STEP_DEBUG is not set.
        abstract setCommandEcho: enabled: bool -> unit
        /// <summary>Sets the action status to failed.
        /// When the action exits it will be with an exit code of 1</summary>
        /// <param name="message">add error issue message</param>
        abstract setFailed: message: U2<string, Error> -> unit
        /// Gets whether Actions Step Debug is on or not
        abstract isDebug: unit -> bool
        /// <summary>Writes debug message to user log</summary>
        /// <param name="message">debug message</param>
        abstract debug: message: string -> unit
        /// <summary>Adds an error issue</summary>
        /// <param name="message">error issue message. Errors will be converted to string via toString()</param>
        /// <param name="properties">optional properties to add to the annotation.</param>
        abstract error: message: U2<string, Error> * ?properties: AnnotationProperties -> unit
        /// <summary>Adds a warning issue</summary>
        /// <param name="message">warning issue message. Errors will be converted to string via toString()</param>
        /// <param name="properties">optional properties to add to the annotation.</param>
        abstract warning: message: U2<string, Error> * ?properties: AnnotationProperties -> unit
        /// <summary>Adds a notice issue</summary>
        /// <param name="message">notice issue message. Errors will be converted to string via toString()</param>
        /// <param name="properties">optional properties to add to the annotation.</param>
        abstract notice: message: U2<string, Error> * ?properties: AnnotationProperties -> unit
        /// <summary>Writes info to log with console.log.</summary>
        /// <param name="message">info message</param>
        abstract info: message: string -> unit
        /// <summary>Begin an output group.
        /// 
        /// Output until the next `groupEnd` will be foldable in this group</summary>
        /// <param name="name">The name of the output group</param>
        abstract startGroup: name: string -> unit
        /// End an output group.
        abstract endGroup: unit -> unit
        /// <summary>Wrap an asynchronous function call in a group.
        /// 
        /// Returns the same type as the function itself.</summary>
        /// <param name="name">The name of the group</param>
        /// <param name="fn">The function to wrap in the group</param>
        abstract group: name: string * fn: (unit -> Promise<'T>) -> Promise<'T>
        /// <summary>Saves state for current action, the state can only be retrieved by this action's post job execution.</summary>
        /// <param name="name">name of the state to store</param>
        /// <param name="value">value to store. Non-string values will be converted to a string via JSON.stringify</param>
        abstract saveState: name: string * value: obj option -> unit
        /// <summary>Gets the value of an state set by this action's main execution.</summary>
        /// <param name="name">name of the state to get</param>
        abstract getState: name: string -> string
        abstract getIDToken: ?aud: string -> Promise<string>

    /// Interface for getInput options
    type [<AllowNullLiteral>] InputOptions =
        /// Optional. Whether the input is required. If required and not present, will throw. Defaults to false
        abstract required: bool option with get, set
        /// Optional. Whether leading/trailing whitespace will be trimmed for the input. Defaults to true
        abstract trimWhitespace: bool option with get, set

    type [<RequireQualifiedAccess>] ExitCode =
        | Success = 0
        | Failure = 1

    /// Optional properties that can be sent with annotatation commands (notice, error, and warning)
    /// See: https://docs.github.com/en/rest/reference/checks#create-a-check-run for more information about annotations.
    type [<AllowNullLiteral>] AnnotationProperties =
        /// A title for the annotation.
        abstract title: string option with get, set
        /// The path of the file for which the annotation should be created.
        abstract file: string option with get, set
        /// The start line for the annotation.
        abstract startLine: float option with get, set
        /// The end line for the annotation. Defaults to `startLine` when `startLine` is provided.
        abstract endLine: float option with get, set
        /// The start column for the annotation. Cannot be sent when `startLine` and `endLine` are different values.
        abstract startColumn: float option with get, set
        /// The start column for the annotation. Cannot be sent when `startLine` and `endLine` are different values.
        /// Defaults to `startColumn` when `startColumn` is provided.
        abstract endColumn: float option with get, set

module File_command =

    type [<AllowNullLiteral>] IExports =
        abstract issueCommand: command: string * message: obj option -> unit

module Oidc_utils =

    type [<AllowNullLiteral>] IExports =
        abstract OidcClient: OidcClientStatic

    type [<AllowNullLiteral>] OidcClient =
        interface end

    type [<AllowNullLiteral>] OidcClientStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> OidcClient
        abstract getIDToken: ?audience: string -> Promise<string>

module Path_utils =

    type [<AllowNullLiteral>] IExports =
        /// toPosixPath converts the given path to the posix form. On Windows, \\ will be
        /// replaced with /.
        abstract toPosixPath: pth: string -> string
        /// toWin32Path converts the given path to the win32 form. On Linux, / will be
        /// replaced with \\.
        abstract toWin32Path: pth: string -> string
        /// <summary>toPlatformPath converts the given path to a platform-specific path. It does
        /// this by replacing instances of / and \ with the platform-specific path
        /// separator.</summary>
        /// <param name="pth">The path to platformize.</param>
        abstract toPlatformPath: pth: string -> string

module Summary =

    type [<AllowNullLiteral>] IExports =
        abstract SUMMARY_ENV_VAR: obj
        abstract SUMMARY_DOCS_URL: obj
        abstract Summary: SummaryStatic
        abstract markdownSummary: Summary
        abstract summary: Summary

    type SummaryTableRow =
        ResizeArray<U2<SummaryTableCell, string>>

    type [<AllowNullLiteral>] SummaryTableCell =
        /// Cell content
        abstract data: string with get, set
        /// Render cell as header
        /// (optional) default: false
        abstract header: bool option with get, set
        /// Number of columns the cell extends
        /// (optional) default: '1'
        abstract colspan: string option with get, set
        /// Number of rows the cell extends
        /// (optional) default: '1'
        abstract rowspan: string option with get, set

    type [<AllowNullLiteral>] SummaryImageOptions =
        /// The width of the image in pixels. Must be an integer without a unit.
        /// (optional)
        abstract width: string option with get, set
        /// The height of the image in pixels. Must be an integer without a unit.
        /// (optional)
        abstract height: string option with get, set

    type [<AllowNullLiteral>] SummaryWriteOptions =
        /// Replace all existing content in summary file with buffer contents
        /// (optional) default: false
        abstract overwrite: bool option with get, set

    type [<AllowNullLiteral>] Summary =
        /// <summary>Writes text in the buffer to the summary buffer file and empties buffer. Will append by default.</summary>
        /// <param name="options">(optional) options for write operation</param>
        abstract write: ?options: SummaryWriteOptions -> Promise<Summary>
        /// Clears the summary buffer and wipes the summary file
        abstract clear: unit -> Promise<Summary>
        /// Returns the current summary buffer as a string
        abstract stringify: unit -> string
        /// If the summary buffer is empty
        abstract isEmptyBuffer: unit -> bool
        /// Resets the summary buffer without writing to summary file
        abstract emptyBuffer: unit -> Summary
        /// <summary>Adds raw text to the summary buffer</summary>
        /// <param name="text">content to add</param>
        /// <param name="addEOL">(optional) append an EOL to the raw text (default: false)</param>
        abstract addRaw: text: string * ?addEOL: bool -> Summary
        /// Adds the operating system-specific end-of-line marker to the buffer
        abstract addEOL: unit -> Summary
        /// <summary>Adds an HTML codeblock to the summary buffer</summary>
        /// <param name="code">content to render within fenced code block</param>
        /// <param name="lang">(optional) language to syntax highlight code</param>
        abstract addCodeBlock: code: string * ?lang: string -> Summary
        /// <summary>Adds an HTML list to the summary buffer</summary>
        /// <param name="items">list of items to render</param>
        /// <param name="ordered">(optional) if the rendered list should be ordered or not (default: false)</param>
        abstract addList: items: ResizeArray<string> * ?ordered: bool -> Summary
        /// <summary>Adds an HTML table to the summary buffer</summary>
        /// <param name="rows">table rows</param>
        abstract addTable: rows: ResizeArray<SummaryTableRow> -> Summary
        /// <summary>Adds a collapsable HTML details element to the summary buffer</summary>
        /// <param name="label">text for the closed state</param>
        /// <param name="content">collapsable content</param>
        abstract addDetails: label: string * content: string -> Summary
        /// <summary>Adds an HTML image tag to the summary buffer</summary>
        /// <param name="src">path to the image you to embed</param>
        /// <param name="alt">text description of the image</param>
        /// <param name="options">(optional) addition image attributes</param>
        abstract addImage: src: string * alt: string * ?options: SummaryImageOptions -> Summary
        /// <summary>Adds an HTML section heading element</summary>
        /// <param name="text">heading text</param>
        /// <param name="level">(optional) the heading level, default: 1</param>
        abstract addHeading: text: string * ?level: U2<float, string> -> Summary
        /// Adds an HTML thematic break (<hr>) to the summary buffer
        abstract addSeparator: unit -> Summary
        /// Adds an HTML line break (<br>) to the summary buffer
        abstract addBreak: unit -> Summary
        /// <summary>Adds an HTML blockquote to the summary buffer</summary>
        /// <param name="text">quote text</param>
        /// <param name="cite">(optional) citation url</param>
        abstract addQuote: text: string * ?cite: string -> Summary
        /// <summary>Adds an HTML anchor tag to the summary buffer</summary>
        /// <param name="text">link text/content</param>
        /// <param name="href">hyperlink</param>
        abstract addLink: text: string * href: string -> Summary

    type [<AllowNullLiteral>] SummaryStatic =
        [<Emit "new $0($1...)">] abstract Create: unit -> Summary

module Utils =
    type AnnotationProperties = Core.AnnotationProperties
    type CommandProperties = Command.CommandProperties

    type [<AllowNullLiteral>] IExports =
        /// <summary>Sanitizes an input into a string so it can be passed into issueCommand safely</summary>
        /// <param name="input">input to sanitize into a string</param>
        abstract toCommandValue: input: obj option -> string
        abstract toCommandProperties: annotationProperties: AnnotationProperties -> CommandProperties
