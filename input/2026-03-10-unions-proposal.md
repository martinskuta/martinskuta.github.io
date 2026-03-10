---
Title: "C# Unions: What they are, how to try them, and whether they'll land in .NET 11"
Date: 2026-03-10
IsPost: true
Tags: [csharp, language, proposal, unions]
Summary: "A practical walk-through of the C# 'Unions' proposal: how to experiment with it, example code, pros/cons, and a candid take on its chances for .NET 11."
---

This is a draft exploring the C# "Unions" language proposal (see the original proposal: https://github.com/dotnet/csharplang/blob/main/proposals/unions.md).

The post covers: how to experiment with the proposal locally, a short example, what is good and what is problematic about the design, and a reasoned estimate of whether the feature will make it into .NET 11.


## TL;DR

- "Unions" are a proposal to allow C# types to behave as closed unions of case types, enabling safer and more concise pattern matching and exhaustiveness checks.
- The feature is still a language proposal; to try it you will likely need a Roslyn build or a nightly/preview compiler that includes the prototype.
- Pros: clearer expression of closed sets of types, exhaustive switch support, better pattern matching ergonomics and nullability tracking. Cons: language surface area increase, subtle conversion and overload-resolution interactions, and potential performance/boxing concerns depending on implementation.


## What "Unions" are (short)

The proposal adds language-level support for union types: types marked with a Union attribute (or using a compact union declaration syntax) are treated specially by the compiler. A union has a set of case types (established by constructors or factory methods) and provides:

- implicit conversions from each case type to the union,
- pattern matching that implicitly unwraps the union's Value for most patterns,
- exhaustiveness checking in switch expressions when all case types are covered,
- improved nullability tracking for the union's contents.

The proposal separates the *union behaviors* (what the compiler recognizes) from an *opinionated union declaration* syntax that makes it concise to declare simple unions.


## How to try it locally

Important: the feature is a language proposal and not guaranteed to be available in released SDKs. There are two practical ways to experiment:

1) Quick local experiment (if you have a preview compiler that already contains a prototype)

- Create a new console app and set the language to preview in the project file:

  <PropertyGroup>
    <LangVersion>preview</LangVersion>
  </PropertyGroup>

- Try compiling example code (below). If the compiler complains about unknown syntax or features, the prototype is not present in your SDK.

2) Try with Roslyn sources / nightly builds (the reliable but heavier route)

- Clone the Roslyn repository and look for any feature branch or PR that implements "unions". If a prototype branch exists you'll need to build the compiler and use that toolset:

  git clone https://github.com/dotnet/roslyn.git
  cd roslyn
  # find a branch or PR that contains unions; otherwise build main if there's a prototype merged
  .\restore.cmd
  .\build.cmd

- After building, the compiler artifacts will be available under the repository's artifacts directories. Use the built csc or configure Visual Studio / your editor to consume the local Roslyn build (see Roslyn's CONTRIBUTING.md for exact instructions). This lets you compile and experiment with the feature even if it's not in the official SDK yet.

Notes and alternatives:

- Visual Studio Preview sometimes ships with newer Roslyn builds—if a prototype is accepted and landed, the Preview channel is the first easy route.
- There may be third-party playgrounds or community branches that provide a quick REPL; search the Roslyn PRs/issues for an implementation to try online if available.


## Example

The following example is adapted from the proposal and demonstrates the basic union pattern.

```csharp
using System.Runtime.CompilerServices;

[Union]
public record struct Pet
{
    // Creation members => case types are Dog and Cat
    public Pet(Dog value) => Value = value;
    public Pet(Cat value) => Value = value;

    // Exposed value (object? typed)
    public object? Value { get; }
}

public class Dog { public string Name { get; set; } }
public class Cat { public string Name { get; set; } }

// Usage
Pet p = new Dog { Name = "Fido" }; // implicit union conversion from Dog

string Describe(Pet pet) => pet switch
{
    Dog d => $"Dog: {d.Name}",
    Cat c => $"Cat: {c.Name}",
    null => "null pet",
    _ => "unknown"
};
```

Notes on the example:

- Pattern matching like `pet is Dog d` is implicitly applied to the union's `Value` rather than the union wrapper itself (so `d` will be the unwrapped Dog instance).
- Switch expressions over a union can be exhaustive if all case types are covered; in that case a default arm is not required.


## What is good about the proposal

- Expressiveness: gives a compact way to represent values that are one of a closed set of types.
- Pattern matching ergonomics: the compiler unwrapping avoids repeated boilerplate and makes matches concise.
- Exhaustiveness: switch expressions become safer because the compiler knows the complete set of case types.
- Separation of concerns: existing types can opt-in to union behavior (via attribute or provider) without relying solely on the shorthand declaration syntax.
- Nullability improvements: the proposal includes rules to better track null state of union contents.


## What is problematic or still TBD

- Implementation complexity: unions interact with conversions, overload resolution, and existing user-defined conversions; corner cases around ambiguous conversions could be confusing.
- Boxing/Performance: depending on how unions are stored and whether non-boxing accessors are used, value-type case types could still cause boxing unless carefully implemented.
- Learnability / Surface area: unions add new patterns and behaviors the C# community will need to learn; the shorthand union declaration is opinionated and may not cover every use case.
- Interop with other features: interactions with pattern combinators, `is`/`as`, generic type inference, and source generators require careful consideration and more real-world experience.


## How likely is it to ship in .NET 11?

Predicting acceptance is inherently uncertain. Factors that matter:

- Active discussion and a championed proposal are necessary preconditions (the proposal is championed—see the csharplang proposal linked above).
- Implementation availability: if Roslyn contains a working prototype and the design is stable, adoption into the language can happen faster.
- Prioritization: the language team must weigh unions against other language investments and runtime impacts.
- Backwards compatibility risk: the team will be conservative about any change that could introduce ambiguous conversions or break existing code.

Bottom-line estimate: medium probability (roughly 40–60%). The feature is valuable and has clear semantics, but the design surface and interactions with existing conversion rules could delay acceptance. If a clear, well-tested Roslyn prototype appears and the community feedback is positive, chances increase substantially.


## Recommended next steps to experiment and contribute

- Read the proposal carefully: https://github.com/dotnet/csharplang/blob/main/proposals/unions.md
- Search the dotnet/roslyn repo for an implementation branch or related PRs; if one exists, try building Roslyn locally and exercising the feature.
- Try writing small libraries that model common union use-cases (result types, optional typed values) to discover ergonomic or API gaps.
- Provide feedback on the proposal or an implementation PR: real-world examples and test cases are the fastest way to influence design.


---

This is a draft — tell me if you'd like the post shortened, expanded with more samples (e.g., non-boxing pattern sample), or committed into a posts folder with a specific slug/date.