import Navbar from './components/Navbar'
import Sidebar from './components/Sidebar'
import Dashboard from './pages/Dashboard'
import Students from './pages/Students'
import Tutors from './pages/Tutors'
import Subjects from './pages/Subjects'
import { Routes, Route } from 'react-router-dom'
import './App.css'


function App() {
  return (
    <div className="app">
      <Navbar />

      <div className="layout">
        <Sidebar />

        <main>
          <Routes>
            <Route path="/" element={<Dashboard />} />
            <Route path="/dashboard" element={<Dashboard />} />
            <Route path="/students" element={<Students />} />
            <Route path="/tutors" element={<Tutors />} />
            <Route path="/subjects" element={<Subjects />} />
          </Routes>
        </main>
      </div>
    </div>
  )
}

export default App