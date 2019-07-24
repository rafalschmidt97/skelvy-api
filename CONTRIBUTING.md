# Contributing Guidelines

We would love for you to contribute to skelvy and help make it even better than it is today! 
As a contributor, here are the guidelines we would like you to follow:

* [Issues and Bugs](#issue)
* [Feature Requests](#feature)
* [Submission Guidelines](#submit)
* [Development Setup](#development)
* [Coding Rules](#rules)
* [Commit Message Guidelines](#commit)

## <a name="issue"></a> Found a Bug?

If you find a bug in the source code, you can help us by
[submitting an issue](#submit-issue) to our [GitHub Repository][github]. Even better, you can
[submit a Pull Request](#submit-pr) with a fix.

## <a name="feature"></a> Missing a Feature?

You can _request_ a new feature by [submitting an issue](#submit-issue) to our GitHub
Repository. If you would like to _implement_ a new feature, please submit an issue with
a proposal for your work first, to be sure that we can use it.
Please consider what kind of change it is:

* For a **Major Feature**, first open an issue and outline your proposal so that it can be
  discussed. This will also allow us to better coordinate our efforts, prevent duplication of work,
  and help you to craft the change so that it is successfully accepted into the project. For your 
  issue name, please prefix your proposal with `[discussion]`, for example "[discussion]: your feature idea".
* **Small Features** can be crafted and directly [submitted as a Pull Request](#submit-pr).

## <a name="submit"></a> Submission Guidelines

### <a name="submit-issue"></a> Submitting an Issue

Before you submit an issue, please search the issue tracker, maybe an issue for your problem 
already exists and the discussion might inform you of workarounds readily available.

### <a name="submit-pr"></a> Submitting a Pull Request (PR)

Before you submit your Pull Request (PR) consider the following guidelines:

1. Search [GitHub][pulls] for an open or closed PR
   that relates to your submission. You don't want to duplicate effort.
1. Fork the repository.
1. Make your changes in a new git branch:

   ```shell
   git checkout -b my-fix-branch master
   ```

1. Create your patch, **including appropriate test cases**.
1. Follow our [Coding Rules](#rules).
1. Run the full test suite
1. Commit your changes using a descriptive commit message that follows our
   [commit message conventions](#commit). Adherence to these conventions
   is necessary because release notes are automatically generated from these messages.

   ```shell
   git commit -a
   ```

   Note: the optional commit `-a` command line option will automatically "add" edited files.

1. Push your branch to GitHub:

   ```shell
   git push origin my-fix-branch
   ```

1. In GitHub, send a pull request to `skelvy-api:master`.

* If we suggest changes then:

  * Make the required updates.
  * Re-run the test suites to ensure tests are still passing.
  * Rebase your branch to upstream and force push to your GitHub repository (this will update your Pull Request):

    ```shell
    git checkout master
    git pull upstream master
    git checkout your-feature-branch
    git rebase upstream/master
  
    Once you have fixed conflicts
  
    git rebase --continue
    git push -f
    ```

That's it! Thank you for your contribution!

#### After your pull request is merged

After your pull request is merged, you can safely delete your branch and pull the changes
from the main (upstream) repository:

* Delete the remote branch on GitHub either through the GitHub web UI or your local shell as follows:

  ```shell
  git push origin --delete my-fix-branch
  ```

* Check out the master branch:

  ```shell
  git checkout master -f
  ```

* Delete the local branch:

  ```shell
  git branch -D my-fix-branch
  ```

* Update your master with the latest upstream version:

  ```shell
  git pull upstream master
  ```

## <a name="development"></a> Development Setup

You will need dotnet core version 2.2+:

1. After cloning the repo, run:

```bash
$ dotnet restore
```

2. Fill keys (marked as REPLACE_WITH_SECRET) in [secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2) or appsettings

3. In order to prepare your environment run `prepare.sh` shell script:

```bash
# docker-compose up
$ sh scripts/prepare.sh // TODO: script is not ready yet
```

### Commonly used scripts

```bash
# add secret for local development (useful to generate secret file as well)
$ dotnet user-secrets set "FACEBOOK_CLIENT_ID" "x" --project src/Skelvy.WebAPI

# set environment as development (it's neccesary for example to add a migration)
set ASPNETCORE_ENVIRONMENT=Development // cmd
$Env:ASPNETCORE_ENVIRONMENT = "Development" // powershell
export ASPNETCORE_ENVIRONMENT=Development // bash

# add migration
$ dotnet ef migrations add Initial --project src/Skelvy.Persistence --verbose

# update database
$ dotnet ef database update --project src/Skelvy.Persistence --verbose

# publish new version
$ docker rmi skelvy/skelvy-api && docker build -t skelvy/skelvy-api . && docker push skelvy/skelvy-api

# run integration tests
# docker is required(!)
$ sh scripts/run-integration.sh // IN PROGRESS
```

### Generating new ssl with Let's Encrypt

```bash
$ sudo certbot certonly --manual --preferred-challenges=dns --email contact.skelvy@gmail.com --server https://acme-v02.api.letsencrypt.org/directory --agree-tos --domain "skelvy.com" --domain "api.skelvy.com" --domain "www.skelvy.com" --domain "www.api.skelvy.com" --work-dir ~/Downloads

$ sudo openssl pkcs12 -inkey privkey.pem -in fullchain.pem -export -out cert.pfx
```

## <a name="rules"></a> Coding Rules

To ensure consistency throughout the source code, keep these rules in mind as you are working:

* We follow [Microsoft's C# Style Guide](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/).

## <a name="commit"></a> Commit Message Guidelines

We have very precise rules over how our git commit messages can be formatted. This leads to **more
readable messages** that are easy to follow when looking through the **project history**.

### Commit Message Format

Each commit message consists of a **header**, a **body** and a **footer**. The header has a special
format that includes a **type**, a **scope** and a **subject**:

```
<type>(<scope>): <subject>
<BLANK LINE>
<body>
<BLANK LINE>
<footer>
```

The **header** is mandatory and the **scope** of the header is optional.

Any line of the commit message cannot be longer 100 characters! This allows the message to be easier
to read on GitHub as well as in various git tools.

Footer should contain a [closing reference to an issue](https://help.github.com/articles/closing-issues-via-commit-messages/) if any.

```
docs(contributing) update list of commit types
bugfix(profile) add missing dto field with photo
```

### Type

Must be one of the following:

* **build**: Changes that affect the build system or external dependencies
* **ci**: Changes to our CI configuration files and scripts
* **docs**: Documentation only changes
* **feature**: A new feature
* **bugfix**: A bug fix
* **performance**: A code change that improves performance
* **refactor**: A code change that neither fixes a bug nor adds a feature
* **style**: Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc)

### Scope

The scope help others in recognising which package was affected.

### Subject

The subject contains succinct description of the change:

* use the imperative, present tense: "change" not "changed" nor "changes"
* don't capitalize first letter
* no dot (.) at the end

### Body

Just as in the **subject**, use the imperative, present tense: "change" not "changed" nor "changes".
The body should include the motivation for the change and contrast this with previous behavior.

### Footer

The footer should contain any information about **Breaking Changes** and is also the place to
reference GitHub issues that this commit **Closes**.

[github]: https://github.com/rafalschmidt97/skelvy-api
[pulls]: https://github.com/rafalschmidt97/skelvy-api/pulls
