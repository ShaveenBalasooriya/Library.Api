- Use the CPM and Central build settings -> Project can get large and there are sub-projects, that would be managing their own packages. One central place to manage all the version. (Note to future self: Yoo make sure to create groups for these when you add Aspire and Testing)

- Do you really want to separate the WebAPI and the Presentation? Look into Composition Root else just simplify it.

- Look into how we can implement something like Pattern Matching. You can wait till you are at the Presentation Layer to look at this.

- Try updating the base entity using the ReferenceEquals in case of we are by chance we are comparing the same entity in the exact heap location. (Performance Optimization)

- Nullable Reference Types only provide compile-time warnings that can be bypassed with the null-forgiving operator (null!), so your static factory methods must still use runtime null guards to keep your domain aggregates truly self-defending
