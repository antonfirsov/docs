# Continuous integration workflow template for desktop apps with GitHub Actions

We know how time consuming it can be to quickly set up continuous integration and continuous deployment workflows (CI/CD) for your WPF and Windows Forms desktop applications.

That's why, in cooperation with the GitHub Actions team, we have released [a starter workflow in GitHub](https://github.com/actions/starter-workflows/blob/master/ci/wpf-dotnet-core.yml, "GitHub Actions Starter Workflows page") to help you quickly set up and showcase DevOps for your applications using the recently released [GitHub Actions](https://github.com/features/actions "GitHub Actions page").

With GitHub Actions, you can quickly and easily automate your software workflows with CI/CD.
* Integrate code changes directly into GitHub to speed up development cycles
* Trigger builds to quickly identify build breaks and create testable debug builds
* Continuously run tests to identify and eliminate bugs
* Automatically build, sign, package and deploy branches that pass tests 
 
The starter workflow template can be added directly to your project in a few simple steps, and with minimal configuration, allowing you to quickly set up a DevOps workflow in GitHub.

The workflow will build, test, sign and package a WPF desktop application built on .NET Core, but can be adapted for Windows Forms or even UWP applications.

To add the workflow:
1. Navigate to the "Actions" tab in your GitHub project.
2. Click the "New workflow" button.
3. Select "Set up this workflow" in the "WPF .NET Core" workflow.
4. Commit the file to your repo.

Once added to your repo, follow the instructions to configure the workflow for your project.

If you have any questions or feedback, please [file issues on GitHub](https://github.com/actions/starter-workflows/issues/new).

![DevOps With GitHub Actions](github-actions-workflow-template.png)
