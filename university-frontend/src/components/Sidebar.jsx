import { Link } from 'react-router-dom'
function Sidebar() {
  return (
    <aside>
      <ul>
        <li><Link to="/dashboard">Dashboard</Link></li>
        <li><Link to="/students">Students</Link></li>
        <li><Link to="/tutors">Tutors</Link></li>
        <li><Link to="/subjects">Subjects</Link></li>
      </ul>
    </aside>
  )
}

export default Sidebar