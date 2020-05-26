<?php

$email = $_POST["email"];
$num = $_POST["number"];

$conn = mysqli_connect('localhost', 'root','t1fkthsl', 'vity-game');
//check that connection happend
if(mysqli_connect_errno())
{
    echo "1: connection failed"; // error code #1 = connection failed
    exit();
}

$sql = "SELECT email FROM player WHERE email ='".$email."'";
$result = $conn->query($sql);

if($result->num_rows > 0){
  while($row = $result->fetch_assoc()) {
    echo "This email is already in use";
  }
}else{
  echo "Email available";
  $sql2 = "INSERT INTO playerTmp (email, randomNum)
        VALUES ('".$email."','".$num."')";
        $conn->query($sql2);
}
$conn->close();

?>
