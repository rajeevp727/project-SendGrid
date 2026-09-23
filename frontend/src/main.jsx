import React from 'react'
import { createRoot } from 'react-dom/client'
import './styles.css'

function App() {
  return (
    <main className="shell">
      <header>
        <p className="eyebrow">PROJECT SENDGRID</p>
        <h1>Communications dashboard</h1>
        <p>Send and monitor transactional email, SMS and WhatsApp messages.</p>
      </header>

      <section className="grid">
        <article><strong>0</strong><span>Messages today</span></article>
        <article><strong>0</strong><span>Delivered</span></article>
        <article><strong>0</strong><span>Failed</span></article>
        <article><strong>3</strong><span>Channels</span></article>
      </section>

      <section className="panel">
        <h2>MVP workspace</h2>
        <p>Provider configuration, message history and usage analytics will appear here.</p>
      </section>
    </main>
  )
}

createRoot(document.getElementById('root')).render(
  <React.StrictMode><App /></React.StrictMode>
)
