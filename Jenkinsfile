pipeline {
  agent any
  options { timestamps() }

  parameters {
    choice(name: 'FILTER_MODE', choices: ['none','category'], description: 'Wie filtern?')
    string(name: 'CATEGORY', defaultValue: 'smoke', description: 'z.B. smoke/regression (bei FILTER_MODE=category)')
  }

  stages {
    stage('Checkout') { steps { checkout scm } }

    stage('Restore') {
      steps { dir('JenkinsTest') { bat 'dotnet restore JenkinsTest.csproj' } }
    }

    stage('Build') {
      steps { dir('JenkinsTest') { bat 'dotnet build JenkinsTest.csproj --configuration Debug --no-restore' } }
    }

    stage('Test') {
      steps {
        dir('JenkinsTest') {
          script {
            def filterArg = ''
            if (params.FILTER_MODE == 'category' && params.CATEGORY?.trim()) {
              filterArg = "--filter \"TestCategory=${params.CATEGORY.trim()}\""
            }
            bat "dotnet test JenkinsTest.csproj --configuration Debug --no-build ${filterArg} --logger \"trx;LogFileName=TestResults.trx\""
          }
        }
      }
    }

    stage('Allure Report') {
      steps { allure([results: [[path: 'JenkinsTest/**/allure-results']]]) }
    }
  }

  post {
    always {
      archiveArtifacts artifacts: 'JenkinsTest/**/allure-results/**', allowEmptyArchive: true
      junit allowEmptyResults: true, testResults: 'JenkinsTest/**/TestResults/*.trx'
    }
  }
}
