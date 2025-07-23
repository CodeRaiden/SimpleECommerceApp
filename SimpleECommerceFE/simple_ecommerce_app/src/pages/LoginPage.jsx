import React from 'react'
export default function LoginPage({ onLogin }) {
  return (
    <div>
      <h1>Login</h1>
      <button onClick={onLogin}>Sign in (demo)</button>
    </div>
  )
}
