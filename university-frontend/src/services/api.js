const API_URL = 'http://localhost:5137'

export async function getStudents() {
  const response = await fetch(`${API_URL}/api/students`)

  if (!response.ok) {
    throw new Error('Failed to fetch students')
  }

  return response.json()
}

export async function createStudent(student) {
  const response = await fetch(`${API_URL}/api/Students`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(student)
  })

  if (!response.ok) {
    throw new Error('Failed to create student')
  }

  return response.json()
}

export async function deleteStudent(id){
    const response = await fetch(`${API_URL}/api/Students/${id}`, {
    method: 'DELETE'
  })

  if (!response.ok) {
    throw new Error('Failed to Delete student')
  }

}