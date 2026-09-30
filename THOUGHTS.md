- Use the CPM and Central build settings -> Project can get large and there are sub-projects, that would be managing their own packages. One central place to manage all the version. (Note to future self: Yoo make sure to create groups for these when you add Aspire and Testing)

- Do you really want to separate the WebAPI and the Presentation? Look into Composition Root else just simplify it.

- Look into how we can implement something like Pattern Matching. You can wait till you are at the Presentation Layer to look at this.

- Try updating the base entity using the ReferenceEquals in case of we are by chance we are comparing the same entity in the exact heap location. (Performance Optimization)

- Nullable Reference Types only provide compile-time warnings that can be bypassed with the null-forgiving operator (null!), so your static factory methods must still use runtime null guards to keep your domain aggregates truly self-defending (this is neither a design pattern or a OOP concept).

- So apparently the domain layer shouldn't know or call the system time directly. And instead it should be provided a time provider interface, like TimeProvider. The domain should get the time as a plain value.

- Domain Purity is something cool to look into. Domain Purity Vs. Domain Completeness.

- From a UX perspective, should we let the Frontend team, also need to get all of the details about a Book or Member, then send all of those details back to the backend just to make an update? We are basically creating a new object again just for the update. Plus we already have the validations in place to check for the required value in the pipeline and the domain, so should we make it this hard for the Frontend team to make a simple update? Changes the Updates commands to be PUT Partial.

- Okay so gang so past Shaveen over here: So currently in the Logging Behavior you are logging everything in the response request, but when you add authentication you will be working with passwords, so in the command handlers with sensitive details make sure to add a [property: NotLogged] next to the attribute you don't want to log. 🙏🏽

- "Microsoft.EntityFrameworkCore.Database.Command": "Information" change this to Warning later.

- Something pretty cool to learn about is how Aspire handles dev certs and how it creates them, because I'm pretty sure I couldn't use HTTPS when I was using with something like docker build.

- Authentik builds a token's iss from the hostname it was reached on. The browser (Scalar) signs in via localhost:9000, so tokens say iss = http://localhost:9000/.... The API container can't reach localhost:9000 (inside a container, localhost is the container itself), so it has to use the compose service name server:9000. Setting only Authority can't satisfy both. localhost means the key fetch fails, and server means the discovery document's issuer doesn't match the token. The fix is to split the two jobs: MetadataAddress (where to fetch keys, server:9000) and ValidIssuer (which iss to accept, localhost:9000).

- Open question, decide later: should DELETE /api/members/{id:guid} (RemoveMember) stay admin-only, or should there also be a self-service DELETE /api/members/me so a member can delete their own account? Leaning toward both existing side by side (same shape as Borrow: a self path plus an admin-by-id path), but punted on it for now instead of guessing again after getting the UpdateProfile self-vs-admin scoping wrong once already.

- Authentik owns FullName/Email but this API only finds out about a change once the user's token gets refreshed AND they happen to hit an endpoint that calls GetOrCreateCurrentMemberAsync (me, update-profile, or borrow-as-self) - so a name/email change can sit stale for a while. Plan: add a webhook endpoint (something like POST /api/webhooks/authentik/user-updated) that Authentik calls via a Notification Rule + webhook Notification Transport whenever a user record changes, verified with a shared secret so randoms can't hit it, and have it run the same SyncIdentity logic immediately instead of waiting on the next login. Keep the existing lazy sync as a fallback for missed webhooks rather than replacing it outright. Before building: confirm the id Authentik's webhook payload uses to identify the user is actually the same value that ends up as the JWT `sub` claim, otherwise the lookup by IdentityId won't line up.

- Profile data ends up split across two systems: Authentik owns FullName/Email (synced into Member on each request), the API owns PhoneNumber (only editable via PUT /api/members/me) - so there's no single place for a member to update everything about themselves. Two ways to close this: (1) keep the split, unify at the UI level only - one "my profile" screen, an Edit button on name/email deep-links out to Authentik's own settings flow with a ?next= back into the app, phone stays inline via the existing PUT, no backend changes; or (2) move phone into Authentik too - custom user attribute + scope mapping + phone claim, synced in like name/email, and PUT /api/members/me goes away entirely so Member has zero user-editable fields. Ruled out having the API write name/email back to Authentik through its admin API - that means admin credentials sitting in this app, skips email verification, and opens up partial-failure states between the two systems. Real question to settle is whether phone is identity data (reusable by other apps, SMS MFA -> option 2) or library-specific data (-> option 1). Punting until the frontend profile page or custom Authentik flows are actually being built. Side notes either way: email changes in Authentik should require verification (SMTP + an email stage), and IsActive still isn't enforced anywhere so that's its own separate open item.

