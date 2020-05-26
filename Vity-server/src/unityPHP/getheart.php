<?php
$username = $_POST["username"];

$conn = mysqli_connect('localhost', 'root','t1fkthsl', 'vity-game');
//check that connection happend
if(mysqli_connect_errno())
{
    echo "1: connection failed"; // error code #1 = connection failed
    exit();
}

$sql = "SELECT heart FROM player WHERE username ='".$username."'";
$result = $conn->query($sql) or die("2 : Name check query failed");

if($result->num_rows > 0){
  while($row = $result->fetch_assoc()) {
    //echo $row["hash"];
    echo $row["heart"];
  }
}

$conn->close();
?>
