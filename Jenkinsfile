pipeline {
  agent any
  options { timestamps() }

  stages {
    stage('Checkout') {
      steps { checkout scm }
    }

    stage('Restore') {
      steps {
        dir('JenkinsTest') {           // <-- in den Projektordner gehen
          bat 'dotnet restore JenkinsTest.csproj'
        }
      }
    }

    stage('Build') {
      steps {
        dir('JenkinsTest') {
          bat 'dotnet build JenkinsTest.csproj --configuration Debug --no-restore'
        }
      }
    }

    stage('Test') {
      steps {
        dir('JenkinsTest') {
          // Allure.NUnit schreibt die Ergebnisse in allure-results
          bat 'dotnet test JenkinsTest.csproj --configuration Debug --no-build --logger "trx;LogFileName=TestResults.trx"'
        }
      }
    }

    stage('Allure Report') {
      steps {
        // Jenkins Allure-Plugin: Ergebnisse einsammeln (aus dem Projekt-Unterordner)
        allure([
          includeProperties: false,
          jdk: '',
          results: [[path: 'JenkinsTest/**/allure-results']]
        ])
      }
    }
  }

  post {
    always {
      archiveArtifacts allowEmptyArchive: true, artifacts: 'JenkinsTest/**/allure-results/**'
      junit allowEmptyResults: true, testResults: 'JenkinsTest/**/TestResults/*.trx'
    }
  }
}
