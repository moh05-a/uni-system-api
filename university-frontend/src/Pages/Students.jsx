import { useEffect, useState } from 'react'
import { getStudents, createStudent , deleteStudent } from '../services/api'

function Students() {
  const [students, setStudents] = useState([])
  const [loading , setLoading]  = useState(true)
  const [error , setError] = useState('')
  const [name, setName] = useState('')
  const [major, setMajor] = useState('')
  const [gpa, setGpa] = useState('')

  useEffect(() => {
    getStudents()
      .then(data => {
        setStudents(data)
      })
      .catch(error => {
        console.error(error.message)
      })
      .finally(() => {
        setLoading(false)
      })
  }, [])

  if (loading) {
    return <p>Loading students...</p>
  }

  async function handleAddStudent(){
    try{
      await createStudent({
        name : name,
        major: major,
        gpa : Number(gpa)
         })

         setName('')
         setMajor('')
         setGpa('')

         const data = await getStudents()
         setStudents(data)
    }catch(error)
    {
      setError(error.message)
    }
  }

  async function handleDeleteStudent(id){
    try{
      await deleteStudent(id)
      const data = await getStudents() //the database deleted the student but react has to now the new list of students
      setStudents(data)
    }catch(error){
      setError(error.message)

    }
  }

  return (
    <div>
      <div className="page-header">
        <h2>Students</h2>
        <p>Manage university students</p>
      </div>

      <div className = "students-form">
        <h3>ADD Student</h3>
        <input 
        type = "text"
        placeholder = "Name"
        value={name}
        onChange={(e)=> setName(e.target.value)}
        />

        <input 
        type = "text"
        placeholder = "Major"
        value={major}
        onChange={(e)=> setMajor(e.target.value)}
        />

        <input 
        type = "text"
        placeholder = "GPA"
        value={gpa}
        onChange={(e)=> setGpa(e.target.value)}
        />

        <button type="button" onClick={handleAddStudent}>
           ADD STUDENT 
        </button>

      </div>

      <div className="students-table-container">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Name</th>
              <th>Major</th>
              <th>GPA</th>
              <th>Subjects</th>
              <th>Actions</th>

            </tr>
          </thead>

          <tbody>
            {students.map(student => (
              <tr key={student.id}>
                <td>{student.id}</td>
                <td>{student.name}</td>
                <td>{student.major}</td>
                <td>{student.gpa}</td>
                <td>{student.enrolledSubjectNames.length}</td>

                <td> <button onClick={() => handleDeleteStudent(student.id)}>
                   Delete
                  </button>
                  </td>

              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}

export default Students