#pragma once

#include "opencv2/opencv.hpp"

auto toMat(const std::vector<uint8_t>& data, int w, int h , int numPlanes, int bpp) -> cv::Mat;
void toPlanar(cv::Mat& bgr, std::vector<std::uint8_t>& data);
auto resizeKeepAspectRatio(const cv::Mat &input, const cv::Size &dstSize, const cv::Scalar &bgcolor) -> cv::Mat;
auto createDirectory(std::string directory) -> int;
void toARGB(const cv::Mat &input, void *ptr );
