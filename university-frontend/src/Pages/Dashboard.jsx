import StatCard from '../components/StatCard'

function Dashboard() {
  const stats = [
    { title: 'Students', value: 120 },
    { title: 'Tutors', value: 25 },
    { title: 'Subjects', value: 18 }
  ]

  return (
    <div>
      <div className="page-header">
        <h2>Dashboard</h2>
        <p>Overview of the university system</p>
      </div>

      <div className="stats-container">
        {stats.map((stat) => (
          <StatCard
            key={stat.title}
            title={stat.title}
            value={stat.value}
          />
        ))}
      </div>
    </div>
  )
}

export default Dashboard