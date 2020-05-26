<?php

$username = $_POST["username"];

$conn = mysqli_connect('localhost', 'root','t1fkthsl', 'vity-game');
//check that connection happend
if(mysqli_connect_errno())
{
    echo "1: connection failed"; // error code #1 = connection failed
    exit();
}

$sql = "SELECT username FROM player WHERE username ='".$username."'";
$result = $conn->query($sql);

if($result->num_rows > 0){
  while($row = $result->fetch_assoc()) {
    echo "This id is already in use";
  }
}else{
  echo "id available";
}
$conn->close();

?>
