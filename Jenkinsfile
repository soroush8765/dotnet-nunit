pipeline {
  agent any
  options { timestamps() }

  parameters {
    string(name: 'TEST_FILTER', defaultValue: '', description: 'Optional: Run only specific tests (e.g. Category=smoke)')
  }

  stages {

    stage('Checkout') {
      steps {
        checkout scm
      }
    }

    stage('Restore') {
      steps {
        bat 'dotnet restore'
      }
    }

    stage('Build') {
      steps {
        bat 'dotnet build --configuration Debug --no-restore'
      }
    }

    stage('Test') {
      steps {
        bat """
          if "${params.TEST_FILTER}" == "" (
            dotnet test --configuration Debug --no-build --logger "trx;LogFileName=TestResults.trx"
          ) else (
            dotnet test --configuration Debug --no-build --filter "${params.TEST_FILTER}" --logger "trx;LogFileName=TestResults.trx"
          )
        """
      }
    }

    stage('Allure Report') {
      steps {
        allure([
          includeProperties: false,
          jdk: '',
          results: [[path: '**/allure-results']]
        ])
      }
    }
  }

  post {
    always {
      archiveArtifacts artifacts: '**/allure-results/**', allowEmptyArchive: true
      junit allowEmptyResults: true, testResults: '**/TestResults/*.trx'
    }
  }
}
